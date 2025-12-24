using System.Collections.Generic;
using UnityEngine;

public interface IMatchService
{
    /// <summary>
    /// Returns all match groups (3+ in a row or column).
    /// Does NOT modify gem flags.
    /// </summary>
    List<List<IGemModel>> FindMatchGroups();

    /// <summary>
    /// Returns a flat list of all matched gems.
    /// </summary>
    List<IGemModel> FindAllMatches();
}
