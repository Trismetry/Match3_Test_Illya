using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Handles creation of new gem models in empty board cells.
/// Contains no Unity logic.
/// </summary>
public interface IRefillService
{
    /// <summary>
    /// Fills all empty cells with new gem models.
    /// </summary>
    Task Refill();
}
