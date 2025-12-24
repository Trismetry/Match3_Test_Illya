using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Orchestrates a full turn: swap (animated), matches, bombs, destroy, cascade, refill.
/// </summary>
public class TurnService : ITurnService
{
    private readonly IGemSwapService swapService;
    private readonly IBoardService boardService;
    private readonly IMatchService matchService;
    private readonly IBombService bombService;
    private readonly IDestroyService destroyService;
    private readonly ICascadeService cascadeService;
    private readonly IRefillService refillService;
    private readonly IScoreService scoreService;
    private readonly IComboService comboService;
    private readonly IAnimationService animationService;
    private readonly IGemFactory factory;

    public TurnService(
        IGemSwapService swapService,
        IBoardService boardService,
        IMatchService matchService,
        IBombService bombService,
        IDestroyService destroyService,
        ICascadeService cascadeService,
        IRefillService refillService,
        IScoreService scoreService,
        IComboService comboService,
        IAnimationService animationService,
        IGemFactory factory)
    {
        this.swapService = swapService;
        this.boardService = boardService;
        this.matchService = matchService;
        this.bombService = bombService;
        this.destroyService = destroyService;
        this.cascadeService = cascadeService;
        this.refillService = refillService;
        this.scoreService = scoreService;
        this.comboService = comboService;
        this.animationService = animationService;
        this.factory = factory;
    }

    public async Task<bool> RequestSwapAsync(IGemModel source, Vector2Int direction)
    {
        Vector2Int targetPos = source.Position + direction;
        if (!boardService.IsInsideBoard(targetPos))
            return false;

        IGemModel target = boardService.GetGem(targetPos);
        if (target == null)
            return false;

        var sourceView = factory.GetViewForModel(source);
        var targetView = factory.GetViewForModel(target);

        await animationService.AnimateSwap(sourceView, targetView);

        if (!swapService.TrySwap(source, target))
            return false;

        bombService.SetLastSwap(source, target);
        await ProcessMatchesAndExplosions();
        return true;
    }

    public async Task ExecuteTurn(IGemModel a, IGemModel b)
    {
        if (!swapService.TrySwap(a, b))
            return;

        bombService.SetLastSwap(a, b);
        await ProcessMatchesAndExplosions();
    }

    private async Task ProcessMatchesAndExplosions()
    {
        while (true)
        {
            var matches = matchService.FindAllMatches();

            if (matches.Count == 0)
            {
                comboService.ResetCombo();
                return;
            }

            comboService.IncrementCombo();
            int multiplier = comboService.GetComboMultiplier();
            scoreService.AddScore(matches.Count * 10 * multiplier);

            bombService.CreateBombsFromMatches(matches);
            await bombService.ExplodeMatchedBombs(matches);

            destroyService.DestroyMatched();
            await cascadeService.Cascade();
            await refillService.Refill();
        }
    }
}
