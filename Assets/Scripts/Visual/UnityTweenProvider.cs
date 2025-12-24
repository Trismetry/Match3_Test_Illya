using System.Threading.Tasks;
using UnityEngine;

public class UnityTweenProvider : ITweenProvider
{
    public async Task MoveTo(Transform t, Vector3 target, float duration, float delay = 0f)
    {
        if (delay > 0) await Task.Delay((int)(delay * 1000));

        Vector3 start = t.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            t.position = Vector3.Lerp(start, target, progress);
            await Task.Yield();
        }
        t.position = target;
    }

    public async Task ScaleTo(Transform t, Vector3 target, float duration, float delay = 0f)
    {
        if (delay > 0) await Task.Delay((int)(delay * 1000));

        Vector3 start = t.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            t.localScale = Vector3.Lerp(start, target, progress);
            await Task.Yield();
        }
        t.localScale = target;
    }

    public async Task Shake(Transform t, float amplitude, float duration, float delay = 0f)
    {
        if (delay > 0) await Task.Delay((int)(delay * 1000));

        Vector3 original = t.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            t.localPosition = original + (Vector3)Random.insideUnitCircle * amplitude;
            await Task.Yield();
        }
        t.localPosition = original;
    }

    public async Task Fade(SpriteRenderer sr, float target, float duration, float delay = 0f)
    {
        if (delay > 0) await Task.Delay((int)(delay * 1000));

        float start = sr.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            Color c = sr.color;
            c.a = Mathf.Lerp(start, target, progress);
            sr.color = c;
            await Task.Yield();
        }
    }
}

