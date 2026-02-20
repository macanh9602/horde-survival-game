using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Hex grid model using Cube coordinates (x, y, z where x + y + z = 0).
    /// </summary>
    public class HexGridModel : IHexGrid
    {
        public int width;
        public int height;
        public List<Vector3Int> allCells = new();

        public HexGridModel(int width, int height)
        {
            this.width = width;
            this.height = height;
            allCells = GetAllCells();
        }

        /// <summary>
        /// Check if cube coordinates are within grid bounds (offset space).
        /// </summary>
        public bool IsInsideGrid(Vector3Int cube)
        {
            Vector2Int offset = HexMath.CubeToOffset(cube.x, cube.y, cube.z);
            int col = offset.x;
            int row = offset.y;
            return col >= 0 && col < width && row >= 0 && row < height;
        }

        /// <summary>
        /// Gets all 6 neighbors (or fewer at grid edges) in cube coordinates.
        /// </summary>
        public void GetNeighbors(Vector3Int cube, ref List<Vector3Int> neighbors)
        {
            neighbors.Clear();
            foreach (var dir in HexMath.directions)
            {
                Vector3Int neighbor = cube + dir;
                if (IsInsideGrid(neighbor))
                    neighbors.Add(neighbor);
            }
        }

        private List<Vector3Int> GetAllCells()
        {
            List<Vector3Int> result = new();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Vector3Int cube = HexMath.OffsetToCube(col, row);
                    result.Add(cube);
                }
            }
            return result;
        }
    }
}