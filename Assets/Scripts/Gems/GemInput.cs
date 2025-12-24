using UnityEngine;

/// <summary>
/// Handles player input for a single gem.
/// Delegates press and swipe events to ITurnService for orchestration.
/// </summary>
public class GemInput : MonoBehaviour, IGemInput
{
    private SC_Gem gem;
    private ITurnService turnService;

    public void Initialize(SC_Gem gem, ITurnService turnService)
    {
        this.gem = gem;
        this.turnService = turnService;
    }

    public void OnPress(IGemModel model)
    {
        Debug.Log($"Gem pressed at {model.Position}");
    }

    public async void OnSwipe(Vector2 start, Vector2 end, IGemModel model)
    {
        Vector2 delta = end - start;
        if (delta.magnitude < 0.5f)
        {
            Debug.Log("Swipe too small, ignored.");
            return;
        }

        Vector2Int direction = Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
            ? (delta.x > 0 ? Vector2Int.right : Vector2Int.left)
            : (delta.y > 0 ? Vector2Int.up : Vector2Int.down);

        Debug.Log($"Swipe detected {direction} from {model.Position}");

        bool ok = await turnService.RequestSwapAsync(model, direction);
        Debug.Log(ok ? "Swap executed (animated)" : "Swap failed");
    }
}
