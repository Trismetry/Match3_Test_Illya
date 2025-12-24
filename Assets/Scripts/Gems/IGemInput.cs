using UnityEngine;

/// <summary>
/// Abstraction for gem input handling.
/// </summary>
public interface IGemInput
{
    void Initialize(SC_Gem gem, ITurnService turnService);
    void OnPress(IGemModel model);
    void OnSwipe(Vector2 start, Vector2 end, IGemModel model);
}
