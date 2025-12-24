using UnityEngine;

public class GemSwapService : IGemSwapService
{
    private readonly IBoardService boardService;

    public GemSwapService(IBoardService boardService)
    {
        this.boardService = boardService;
    }

    public bool TrySwap(IGemModel gem, Vector2Int direction)
    {
        Vector2Int targetPos = gem.Position + direction;

        if (!boardService.IsInsideBoard(targetPos))
            return false;

        IGemModel other = boardService.GetGem(targetPos);
        if (other == null)
            return false;

        return TrySwap(gem, other);
    }

    public bool TrySwap(IGemModel a, IGemModel b)
    {
        if (a == null || b == null)
            return false;

        // Swap positions in the model
        Vector2Int posA = a.Position;
        Vector2Int posB = b.Position;

        a.Position = posB;
        b.Position = posA;

        boardService.SetGem(posA, b);
        boardService.SetGem(posB, a);

        return true;
    }
}
