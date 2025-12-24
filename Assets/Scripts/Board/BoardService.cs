using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pure board implementation storing gem models in a 2D array.
/// Contains no Unity logic and no gameplay logic.
/// </summary>
public class BoardService : IBoardService
{
    private readonly IGemModel[,] grid;

    public int Width { get; }
    public int Height { get; }

    public BoardService(int width, int height)
    {
        Width = width;
        Height = height;
        grid = new IGemModel[width, height];
    }

    public bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < Width &&
               pos.y >= 0 && pos.y < Height;
    }

    public IGemModel GetGem(Vector2Int pos)
    {
        return grid[pos.x, pos.y];
    }

    public IGemModel GetGem(int x, int y)
    {
        return grid[x, y];
    }

    public void SetGem(Vector2Int pos, IGemModel gem)
    {
        grid[pos.x, pos.y] = gem;

        if (gem != null)
            gem.Position = pos;
    }

    public void Swap(IGemModel a, IGemModel b)
    {
        Vector2Int posA = a.Position;
        Vector2Int posB = b.Position;

        // Swap in the grid
        grid[posA.x, posA.y] = b;
        grid[posB.x, posB.y] = a;

        // Update model positions
        a.Position = posB;
        b.Position = posA;
    }
    public List<IGemModel> GetAllGems() {
        List<IGemModel> gems = new List<IGemModel>();
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                IGemModel gem = GetGem(x, y);
                if (gem != null) gems.Add(gem);
            }
        }
        return gems;
    }
}
