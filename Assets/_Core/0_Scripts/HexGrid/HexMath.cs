
using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Provides mathematical conversions and calculations for hexagonal grids using Cube coordinates.
    /// Cube: (x, y, z) where x + y + z = 0
    /// </summary>
    public static class HexMath
    {
        /// <summary>
        /// Cube coordinate directions (all 6 neighbors).
        /// Constraint: x + y + z = 0
        /// </summary>
        public static readonly Vector3Int[] directions = new Vector3Int[]
        {
                new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1),
                new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1)
        };

        /// <summary>
        /// Converts offset (col, row) to cube coordinates (x, y, z).
        /// </summary>
        public static Vector3Int OffsetToCube(int col, int row)
        {
            int q = col - (row + (row & 1)) / 2;
            int r = row;
            int x = q;
            int z = r;
            int y = -x - z;
            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// Converts cube coordinates (x, y, z) to offset (col, row).
        /// </summary>
        public static Vector2Int CubeToOffset(int x, int y, int z)
        {
            int q = x;
            int r = z;
            int col = q + (r + (r & 1)) / 2;
            int row = r;
            return new Vector2Int(col, row);
        }

        /// <summary>
        /// Converts cube (x, y, z) to axial (q, r) - kept for compatibility.
        /// </summary>
        public static Vector2Int CubeToAxial(int x, int y, int z)
        {
            return new Vector2Int(x, z);
        }

        /// <summary>
        /// Converts axial (q, r) to cube (x, y, z).
        /// </summary>
        public static Vector3Int AxialToCube(int q, int r)
        {
            int x = q;
            int z = r;
            int y = -x - z;
            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// Rounds cube coordinates to the nearest hex.
        /// </summary>
        public static Vector3Int CubeRound(float x, float y, float z)
        {
            int rx = Mathf.RoundToInt(x);
            int ry = Mathf.RoundToInt(y);
            int rz = Mathf.RoundToInt(z);

            float xDiff = Mathf.Abs(rx - x);
            float yDiff = Mathf.Abs(ry - y);
            float zDiff = Mathf.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff)
            {
                rx = -ry - rz;
            }
            else if (yDiff > zDiff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Vector3Int(rx, ry, rz);
        }


    }
}