using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BombService : IBombService
{
    private readonly IBoardService board;
    private readonly IAnimationService animationService;

    private readonly int delayBeforeNeighborsMs = 150;
    private readonly int delayBeforeBombMs = 150;

    private IGemModel lastSwapA;
    private IGemModel lastSwapB;

    public BombService(IBoardService board, IAnimationService animationService)
    {
        this.board = board;
        this.animationService = animationService;
    }

    // Store last swapped gems for correct bomb placement
    public void SetLastSwap(IGemModel a, IGemModel b)
    {
        lastSwapA = a;
        lastSwapB = b;
    }

    // Create bombs from matches (groups of 4+)
    public void CreateBombsFromMatches(List<IGemModel> matches)
    {
        Dictionary<GlobalEnums.GemType, List<IGemModel>> groups = new();

        foreach (var gem in matches)
        {
            if (!groups.ContainsKey(gem.Type))
                groups[gem.Type] = new List<IGemModel>();

            groups[gem.Type].Add(gem);
        }

        foreach (var kvp in groups)
        {
            if (kvp.Value.Count < 4)
                continue;

            IGemModel origin = GetOriginCell(kvp.Value);
            origin.IsBomb = true;
            origin.IsMatched = false; // bomb must not be destroyed immediately
        }
    }

    // Determine origin cell for bomb placement
    private IGemModel GetOriginCell(List<IGemModel> group)
    {
        if (group.Contains(lastSwapA)) return lastSwapA;
        if (group.Contains(lastSwapB)) return lastSwapB;
        return group[0];
    }

    // Explode bombs inside matched gems
    public async Task ExplodeMatchedBombs(List<IGemModel> matches)
    {
        List<IGemModel> bombs = new();

        foreach (var gem in matches)
        {
            if (gem.IsBomb)
                bombs.Add(gem);
        }

        if (bombs.Count == 0)
            return;

        foreach (var bomb in bombs)
            await ExplodeSingleBomb(bomb);
    }

    // Explode a single bomb with delays and animations
    private async Task ExplodeSingleBomb(IGemModel bomb)
    {
        await animationService.AnimateBombExplosion(bomb.View);

        await Task.Delay(delayBeforeNeighborsMs);

        List<IGemModel> neighbors = GetBlastArea(bomb);
        foreach (var gem in neighbors)
        {
            gem.IsMatched = true;
            await animationService.AnimateNeighborExplosion(gem.View);
        }

        await Task.Delay(delayBeforeBombMs);

        bomb.IsMatched = true;
        await animationService.AnimateBombFinalDestroy(bomb.View);
    }

    // Calculate blast area (cross + diagonals + extended cross)
    private List<IGemModel> GetBlastArea(IGemModel bomb)
    {
        List<IGemModel> result = new();
        Vector2Int c = bomb.Position;

        Vector2Int[] offsets =
        {
            new(0,0),
            new(1,0), new(-1,0), new(0,1), new(0,-1),
            new(1,1), new(1,-1), new(-1,1), new(-1,-1),
            new(2,0), new(-2,0), new(0,2), new(0,-2)
        };

        foreach (var o in offsets)
        {
            Vector2Int pos = c + o;
            if (!board.IsInsideBoard(pos))
                continue;

            IGemModel gem = board.GetGem(pos);
            if (gem != null)
                result.Add(gem);
        }

        return result;
    }
}
