using UnityEngine;
using System.Threading.Tasks;

public class GemAnimator : MonoBehaviour, IGemAnimator
{
    public async Task AnimateMove(Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;
        transform.position = from;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(from, to, t);
            await Task.Yield();
        }

        transform.position = to;
    }

    public async Task AnimateDestroy(float duration)
    {
        float t = 0f;
        Vector3 startScale = transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            await Task.Yield();
        }
    }
}
