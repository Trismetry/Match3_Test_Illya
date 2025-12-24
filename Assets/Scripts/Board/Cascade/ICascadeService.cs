using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Handles gravity-like falling of gems after matched ones are removed.
/// Contains no Unity logic.
/// </summary>
public interface ICascadeService
{
    /// <summary>
    /// Moves gems down to fill empty spaces.
    /// </summary>
    Task<List<GemMove>> Cascade();
}
