using System.Collections.Generic;
using UnityEngine;

public class MatchService : IMatchService
{
    private readonly IBoardService board;

    public MatchService(IBoardService board)
    {
        this.board = board;
    }

    public List<List<IGemModel>> FindMatchGroups()
    {
        List<List<IGemModel>> groups = new();

        bool[,] visited = new bool[board.Width, board.Height];

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                IGemModel gem = board.GetGem(x, y);
                if (gem == null)
                    continue;

                // Horizontal
                var h = FindHorizontalMatch(x, y);
                if (h.Count >= 3)
                    groups.Add(h);

                // Vertical
                var v = FindVerticalMatch(x, y);
                if (v.Count >= 3)
                    groups.Add(v);
            }
        }

        return groups;
    }

    public List<IGemModel> FindAllMatches()
    {
        var groups = FindMatchGroups();
        HashSet<IGemModel> unique = new();

        foreach (var group in groups)
            foreach (var gem in group)
                unique.Add(gem);

        return new List<IGemModel>(unique);
    }

    private List<IGemModel> FindHorizontalMatch(int x, int y)
    {
        List<IGemModel> result = new();
        IGemModel center = board.GetGem(x, y);
        if (center == null)
            return result;

        result.Add(center);

        // Left
        for (int i = x - 1; i >= 0; i--)
        {
            IGemModel g = board.GetGem(i, y);
            if (g != null && g.Type == center.Type)
                result.Add(g);
            else
                break;
        }

        // Right
        for (int i = x + 1; i < board.Width; i++)
        {
            IGemModel g = board.GetGem(i, y);
            if (g != null && g.Type == center.Type)
                result.Add(g);
            else
                break;
        }

        return result;
    }

    private List<IGemModel> FindVerticalMatch(int x, int y)
    {
        List<IGemModel> result = new();
        IGemModel center = board.GetGem(x, y);
        if (center == null)
            return result;

        result.Add(center);

        // Down
        for (int i = y - 1; i >= 0; i--)
        {
            IGemModel g = board.GetGem(x, i);
            if (g != null && g.Type == center.Type)
                result.Add(g);
            else
                break;
        }

        // Up
        for (int i = y + 1; i < board.Height; i++)
        {
            IGemModel g = board.GetGem(x, i);
            if (g != null && g.Type == center.Type)
                result.Add(g);
            else
                break;
        }

        return result;
    }
}
