using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBombService
{
    /// <summary>
    /// Stores the last swapped pair to determine the origin cell for bomb creation.
    /// </summary>
    void SetLastSwap(IGemModel a, IGemModel b);

    /// <summary>
    /// Creates bombs from match groups (4+).
    /// </summary>
    void CreateBombsFromMatches(List<IGemModel> matches);

    /// <summary>
    /// Explodes all bombs inside the matched list.
    /// Handles delays, animations, and chain reactions.
    /// </summary>
    Task ExplodeMatchedBombs(List<IGemModel> matches);
}

