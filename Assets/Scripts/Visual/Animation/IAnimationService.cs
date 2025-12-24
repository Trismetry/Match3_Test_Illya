using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IAnimationService
{
    // Core motions
    Task AnimateSwap(SC_Gem a, SC_Gem b, CancellationToken ct = default);
    Task AnimateGemMove(SC_Gem gem, Vector2Int from, Vector2Int to, float delay = 0f, CancellationToken ct = default);
    Task AnimateSpawn(SC_Gem gem, float delay = 0f, CancellationToken ct = default);

    // Bomb lifecycle
    Task AnimateBombExplosion(SC_Gem bomb, CancellationToken ct = default);          // center burst (FX only)
    Task AnimateNeighborExplosion(SC_Gem neighbor, CancellationToken ct = default);  // hit flash/shake
    Task AnimateBombFinalDestroy(SC_Gem bomb, CancellationToken ct = default);       // bomb vanish (FX)

    // Regular destroy
    Task AnimateDestroy(SC_Gem gem, CancellationToken ct = default);

    // Utility batch helpers (no logic, pure scheduling)
    Task AnimateBatchSequential(params Task[] animations);
    Task AnimateBatchParallel(params Task[] animations);
}