using UnityEngine;

public interface IGemSwapService
{
    /// <summary>
    /// Attempts to swap two gems on the board.
    /// Returns true if swap is valid and performed.
    /// </summary>
    bool TrySwap(IGemModel a, IGemModel b);

    /// <summary>
    /// Attempts to swap a gem in a direction (used by input).
    /// Returns true if swap is valid and performed.
    /// </summary>
    bool TrySwap(IGemModel gem, Vector2Int direction);
}
