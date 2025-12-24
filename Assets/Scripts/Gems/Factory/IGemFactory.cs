using UnityEngine;

/// <summary>
/// Creates gem models and full gem Unity objects.
/// </summary>
public interface IGemFactory
{
    IGemModel CreateModel(Vector2Int position);
    SC_Gem CreateGem(Vector2Int position);

    // Maps models to views (used by TurnService for animation).
    SC_Gem GetViewForModel(IGemModel model);

    // Late injection of TurnService.
    void SetTurnService(ITurnService turnService);
}
