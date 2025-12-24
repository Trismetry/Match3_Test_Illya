using UnityEngine;

/// <summary>
/// Interface for the visual representation of a gem.
/// Handles sprite, effects, animations and position updates.
/// Contains no game logic.
/// </summary>
public interface IGemView
{
    /// <summary>Updates the gem's visual position smoothly on the grid.</summary>
    void UpdatePosition(Vector2Int gridPos, float speed);

    /// <summary>Sets the sprite for this gem.</summary>
    void SetSprite(Sprite sprite);

    /// <summary>Configures sorting layer and order for rendering.</summary>
    void ConfigureSorting(string layerName, int order);

    /// <summary>Plays the destroy effect (particle, animation, etc.).</summary>
    void PlayDestroyEffect();

    /// <summary>Plays the bomb explosion effect.</summary>
    void PlayBombEffect();
}
