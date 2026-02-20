using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Rectangular grid model for bench (1 row x 8 columns)
    /// </summary>
    public class BenchGridModel
    {
        public int width;
        public int height;
        public List<Vector2Int> allCells = new();

        public BenchGridModel(int width, int height)
        {
            this.width = width;
            this.height = height;
            allCells = GetAllCells();
        }

        /// <summary>
        /// Check if cell coordinates are within grid bounds
        /// </summary>
        public bool IsInsideGrid(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
        }

        /// <summary>
        /// Check if cell coordinates are within grid bounds
        /// </summary>
        public bool IsInsideGrid(int col, int row)
        {
            return col >= 0 && col < width && row >= 0 && row < height;
        }

        /// <summary>
        /// Gets all neighbor cells (left and right for bench grid)
        /// </summary>
        public void GetNeighbors(Vector2Int cell, ref List<Vector2Int> neighbors)
        {
            neighbors.Clear();

            // Left neighbor
            if (cell.x > 0)
                neighbors.Add(new Vector2Int(cell.x - 1, cell.y));

            // Right neighbor
            if (cell.x < width - 1)
                neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
        }

        private List<Vector2Int> GetAllCells()
        {
            List<Vector2Int> result = new();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    result.Add(new Vector2Int(col, row));
                }
            }
            return result;
        }
    }
}
