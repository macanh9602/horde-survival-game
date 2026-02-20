using System;
using System.Collections.Generic;
using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Pathfinder using BFS with cube coordinates.
    /// </summary>
    public class HexPathFinder
    {
        private Queue<Vector3Int> queue = new();
        private Dictionary<Vector3Int, Vector3Int> cameFrom = new();
        private List<Vector3Int> neighbors = new();
        private IHexGrid grid;

        public HexPathFinder(IHexGrid grid)
        {
            this.grid = grid;
        }

        /// <summary>
        /// Finds path using BFS in cube coordinate space.
        /// </summary>
        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target, Func<Vector3Int, CellState> getCellDetail)
        {
            if (!grid.IsInsideGrid(start) || !grid.IsInsideGrid(target) || !getCellDetail(target).Walkable) return new List<Vector3Int>();
            queue.Clear();
            cameFrom.Clear();
            List<Vector3Int> result = new();

            if (start == target)
            {
                result.Add(start);
                return result;
            }

            queue.Enqueue(start);
            cameFrom[start] = start;

            bool found = false;
            Debug.Log($"Starting BFS from {start} to {target}");
            while (queue.Count > 0)
            {
                Vector3Int center = queue.Dequeue();
                grid.GetNeighbors(center, ref neighbors);
                foreach (var neighbor in neighbors)
                {
                    if (cameFrom.ContainsKey(neighbor)) continue;
                    CellState detail = getCellDetail(neighbor);
                    if (!detail.Walkable) continue;
                    cameFrom[neighbor] = center;
                    if (neighbor == target)
                    {
                        found = true;
                        break;
                    }

                    queue.Enqueue(neighbor);
                }

                if (found) break;
            }

            if (!found) return result;

            Vector3Int currentStep = target;
            while (currentStep != start)
            {
                result.Add(currentStep);
                currentStep = cameFrom[currentStep];
            }
            result.Add(start);
            result.Reverse();

            return result;
        }
    }
}