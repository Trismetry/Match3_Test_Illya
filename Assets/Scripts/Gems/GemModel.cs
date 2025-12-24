using UnityEngine;

public class GemModel : IGemModel
{
    public Vector2Int Position { get; set; }
    public GlobalEnums.GemType Type { get; private set; }
    public bool IsMatched { get; set; }
    public bool IsBomb { get; set; }
    public int BlastSize { get; private set; }

    // Link to visual representation
    public SC_Gem View { get; set; }

    public GemModel(Vector2Int position, GlobalEnums.GemType type, bool isBomb = false, int blastSize = 1)
    {
        Position = position;
        Type = type;
        IsBomb = isBomb;
        BlastSize = blastSize;
        IsMatched = false;
    }
}
