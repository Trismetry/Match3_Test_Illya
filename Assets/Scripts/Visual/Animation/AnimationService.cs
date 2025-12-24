using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationService : IAnimationService
{
    private readonly ITweenProvider tween;
    private readonly IEffectFactory effects;

    // Tunables (no gameplay constants)
    private readonly float swapDuration = 0.15f;
    private readonly float moveDurationPerCell = 0.08f;
    private readonly float spawnDuration = 0.12f;
    private readonly float bombCenterDuration = 0.2f;
    private readonly float neighborHitDuration = 0.12f;
    private readonly float destroyDuration = 0.15f;

    public AnimationService(ITweenProvider tween, IEffectFactory effects)
    {
        this.tween = tween;
        this.effects = effects;
    }

    public async Task AnimateSwap(SC_Gem a, SC_Gem b, CancellationToken ct = default)
    {
        var ta = a.transform;
        var tb = b.transform;

        Vector3 pa = ta.localPosition;
        Vector3 pb = tb.localPosition;

        // Cross-move in parallel
        await Task.WhenAll(
            tween.MoveTo(ta, pb, swapDuration),
            tween.MoveTo(tb, pa, swapDuration)
        );
    }

    public async Task AnimateGemMove(SC_Gem gem, Vector2Int from, Vector2Int to, float delay = 0f, CancellationToken ct = default)
    {
        var t = gem.transform;
        float distance = Mathf.Abs(to.y - from.y) + Mathf.Abs(to.x - from.x);
        float duration = Mathf.Max(moveDurationPerCell * distance, 0.05f);

        Vector3 target = new(to.x, to.y, t.localPosition.z);
        await tween.MoveTo(t, target, duration, delay);
    }

    public async Task AnimateSpawn(SC_Gem gem, float delay = 0f, CancellationToken ct = default)
    {
        var t = gem.transform;
        var sr = gem.GetComponent<SpriteRenderer>();

        t.localScale = Vector3.zero;
        await Task.WhenAll(
            tween.ScaleTo(t, Vector3.one, spawnDuration, delay),
            sr != null ? tween.Fade(sr, 1f, spawnDuration, delay) : Task.CompletedTask
        );
    }

    public async Task AnimateBombExplosion(SC_Gem bomb, CancellationToken ct = default)
    {
        var pos = bomb.transform.position;
        var handle = effects.PlayAt("fx_bomb_center", pos);
        await tween.ScaleTo(bomb.transform, new Vector3(1.2f, 1.2f, 1f), bombCenterDuration * 0.5f);
        await tween.ScaleTo(bomb.transform, Vector3.one, bombCenterDuration * 0.5f);
        handle?.Stop();
    }

    public async Task AnimateNeighborExplosion(SC_Gem neighbor, CancellationToken ct = default)
    {
        await tween.Shake(neighbor.transform, amplitude: 0.15f, duration: neighborHitDuration);
    }

    public async Task AnimateBombFinalDestroy(SC_Gem bomb, CancellationToken ct = default)
    {
        var sr = bomb.GetComponent<SpriteRenderer>();
        await Task.WhenAll(
            tween.ScaleTo(bomb.transform, new Vector3(0.6f, 0.6f, 1f), destroyDuration),
            sr != null ? tween.Fade(sr, 0f, destroyDuration) : Task.CompletedTask
        );
    }

    public async Task AnimateDestroy(SC_Gem gem, CancellationToken ct = default)
    {
        var sr = gem.GetComponent<SpriteRenderer>();
        await Task.WhenAll(
            tween.ScaleTo(gem.transform, new Vector3(0.7f, 0.7f, 1f), destroyDuration),
            sr != null ? tween.Fade(sr, 0f, destroyDuration) : Task.CompletedTask
        );
    }

    public Task AnimateBatchSequential(params Task[] animations)
        => animations.Length == 0 ? Task.CompletedTask : RunSequential(animations);

    public Task AnimateBatchParallel(params Task[] animations)
        => Task.WhenAll(animations);

    private static async Task RunSequential(Task[] tasks)
    {
        foreach (var t in tasks) await t;
    }
}
