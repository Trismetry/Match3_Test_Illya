using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Coordinates the full turn lifecycle:
/// Swap → Match → Bomb → Destroy → Cascade → Refill
/// </summary>
public interface ITurnService
{
    /// <summary>
    /// Executes a full turn after a swap.
    /// </summary>
    Task ExecuteTurn(IGemModel a, IGemModel b);

    Task<bool> RequestSwapAsync(IGemModel source, Vector2Int direction);
}
