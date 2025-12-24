/// <summary>
/// High-level controller that manages turn execution and animations.
/// </summary>
public interface IGameController
{
    /// <summary>
    /// Called when a swap between two gems is requested by input.
    /// </summary>
    void OnSwapRequested(IGemModel a, IGemModel b);
}
