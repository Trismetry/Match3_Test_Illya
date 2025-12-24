using UnityEngine;

/// <summary>
/// Thin Unity component representing a gem on the board.
/// Delegates all logic to injected interfaces (model, view, input).
/// </summary>
public class SC_Gem : MonoBehaviour
{
    private IGemModel model;
    private IGemView view;
    private IGemInput input;

    private Vector2 firstTouch;
    private bool isPressed;

    /// <summary>
    /// Exposes the gem's model for external services.
    /// </summary>
    public IGemModel Model => model;

    /// <summary>
    /// Initializes the gem with its model, view and input handlers.
    /// </summary>
    public void Initialize(IGemModel model, IGemView view, IGemInput input)
    {
        this.model = model;
        this.view = view;
        this.input = input;

        // If input is a MonoBehaviour, attach it to this GameObject
        if (input is MonoBehaviour mb)
        {
            mb.transform.SetParent(this.transform, false);
        }
    }

    /// <summary>
    /// Resets the gem view to a clean reusable state.
    /// </summary>
    public void ResetView()
    {
        // Clear model reference
        model = null;

        // Reset transform
        transform.localScale = Vector3.one;

        // Optional: reset animations, highlight, selection, etc.
    }

    private void Update()
    {
        if (model == null || view == null) return;

        // Update visual position only (no logic here)
        view.UpdatePosition(model.Position, SC_GameVariables.Instance.gemSpeed);

        // Handle mouse release
        if (isPressed && Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            Vector2 releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Delegate swipe handling to input interface
            input.OnSwipe(firstTouch, releasePos, model);
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
        firstTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Notify input handler about press
        input.OnPress(model);
    }
}
