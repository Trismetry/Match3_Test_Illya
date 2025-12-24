using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstracts the board grid and gem placement.
/// Contains no Unity logic.
/// </summary>
public interface IBoardService
{
    int Width { get; }
    int Height { get; }

    /// <summary>Returns true if the position is inside the board.</summary>
    bool IsInsideBoard(Vector2Int pos);

    /// <summary>Gets the gem model at the given position.</summary>
    IGemModel GetGem(Vector2Int pos);

    /// <summary>Gets the gem model at the given coordinates.</summary>
    IGemModel GetGem(int x, int y);

    /// <summary>Places a gem model at the given position.</summary>
    void SetGem(Vector2Int pos, IGemModel gem);

    /// <summary>Swaps two gem models on the board.</summary>
    void Swap(IGemModel a, IGemModel b);
    /// <summary>Get all Gems.</summary>
    List<IGemModel> GetAllGems();
}
