using UnityEngine;

public interface IGemModel
{
    Vector2Int Position { get; set; }
    GlobalEnums.GemType Type { get; }
    bool IsMatched { get; set; }
    bool IsBomb { get; set; }

    // new link to visual part
    SC_Gem View { get; set; }
}
