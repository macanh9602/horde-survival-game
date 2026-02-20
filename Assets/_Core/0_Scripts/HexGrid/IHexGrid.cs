using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Interface for hex grid operations using Cube coordinates.
    /// </summary>
    public interface IHexGrid
    {
        /// <summary>
        /// Checks if cube coordinates are within grid bounds.
        /// </summary>
        public bool IsInsideGrid(Vector3Int cube);

        /// <summary>
        /// Gets all valid neighbors for a cube cell.
        /// </summary>
        public void GetNeighbors(Vector3Int cube, ref List<Vector3Int> neighbors);
    }
}