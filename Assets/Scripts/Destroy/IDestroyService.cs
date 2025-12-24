using System.Collections.Generic;

public interface IDestroyService
{
    /// <summary>
    /// Removes all matched non-bomb gems from the board.
    /// Returns destroyed models.
    /// </summary>
    List<IGemModel> DestroyMatched();
}
