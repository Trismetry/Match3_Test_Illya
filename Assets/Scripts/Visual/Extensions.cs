using System.Threading.Tasks;
using UnityEngine;

public interface ITweenProvider
{
    Task MoveTo(Transform t, Vector3 target, float duration, float delay = 0f);
    Task ScaleTo(Transform t, Vector3 target, float duration, float delay = 0f);
    Task Shake(Transform t, float amplitude, float duration, float delay = 0f);
    Task Fade(SpriteRenderer sr, float target, float duration, float delay = 0f);
}

public interface IEffectFactory
{
    // Pure visual prefabs/effects creation; returns disposable FX handlers
    IEffectHandle PlayAt(string fxId, Vector3 worldPos);
}

public interface IEffectHandle
{
    void Stop();
}
