using UnityEngine;

/// <summary>
/// Concrete visual implementation of IGemView.
/// Handles sprite rendering, effects and smooth movement.
/// Contains no gameplay logic.
/// </summary>
public class GemView : MonoBehaviour, IGemView
{
    private Transform cachedTransform;
    private SpriteRenderer spriteRenderer;

    [Header("Effects")]
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private GameObject bombEffect;
    public void Initialize(Sprite sprite)
    {
        if (!TryGetComponent(out spriteRenderer))
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        spriteRenderer.sortingLayerName = "Gems";
        spriteRenderer.sortingOrder = 6;
        spriteRenderer.sprite = sprite;

        Debug.Log($"GemView initialized with sprite: {sprite.name}");
    }

    private void Awake()
    {
        // Cache transform and sprite renderer immediately
        cachedTransform = transform;
        TryGetComponent(out spriteRenderer);

        // Ensure a SpriteRenderer exists
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        // Configure sorting layer and order so gems always render above background
        spriteRenderer.sortingLayerName = "Gems";
        spriteRenderer.sortingOrder = 6;
    }

    /// <summary>
    /// Smoothly updates gem position towards the target grid coordinates.
    /// </summary>
    public void UpdatePosition(Vector2Int gridPos, float speed)
    {
        Vector3 target = new(gridPos.x, gridPos.y, 0f);
        cachedTransform.position = Vector3.Lerp(cachedTransform.position, target, speed * Time.deltaTime);
    }

    /// <summary>
    /// Assigns the correct sprite to the gem.
    /// </summary>
    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
            Debug.Log($"GemView: sprite assigned -> {sprite.name}");
        }
    }

    /// <summary>
    /// Plays destroy effect when gem is removed.
    /// </summary>
    public void PlayDestroyEffect()
    {
        if (destroyEffect != null)
            Instantiate(destroyEffect, cachedTransform.position, Quaternion.identity);
    }

    /// <summary>
    /// Plays bomb effect when bomb gem is triggered.
    /// </summary>
    public void PlayBombEffect()
    {
        if (bombEffect != null)
            Instantiate(bombEffect, cachedTransform.position, Quaternion.identity);
    }
    public void ConfigureSorting(string layerName, int order)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = layerName;
            spriteRenderer.sortingOrder = order;
        }
    }

}
