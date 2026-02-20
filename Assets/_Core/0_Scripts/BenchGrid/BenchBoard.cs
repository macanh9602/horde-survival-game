using System.Collections.Generic;
using UnityEngine;
using VTLTools;

namespace DucDevGame
{
    /// <summary>
    /// Manages the bench grid system (1 row x 8 columns for unit placement)
    /// </summary>
    public class BenchBoard : Singleton<BenchBoard>
    {
        public BenchGridModel gridModel;
        [SerializeField] private BenchGridContext gridContext;
        [SerializeField] private BenchGridView gridView;
        
        private Dictionary<Vector2Int, BenchCellState> cells;

        private void Start()
        {
            if (gridContext != null && gridContext.GridData != null)
            {
                gridModel = new BenchGridModel(
                    gridContext.GridData.width, 
                    gridContext.GridData.height
                );
                
                gridContext.CacheOffsets();
                InitializeCells();
            }
        }

        private void InitializeCells()
        {
            cells = new Dictionary<Vector2Int, BenchCellState>();
            foreach (var cell in gridModel.allCells)
            {
                cells[cell] = new BenchCellState 
                { 
                    IsOccupied = false,
                    OccupyingUnit = null
                };
            }
        }

        /// <summary>
        /// Gets cell state
        /// </summary>
        public BenchCellState GetCellState(Vector2Int cell)
        {
            if (!cells.TryGetValue(cell, out var state))
            {
                return new BenchCellState { IsOccupied = true }; // Invalid cells are considered occupied
            }
            return state;
        }

        /// <summary>
        /// Sets a cell as occupied by a unit
        /// </summary>
        public bool OccupyCell(Vector2Int cell, GameObject unit)
        {
            if (!cells.ContainsKey(cell))
                return false;

            var state = cells[cell];
            if (state.IsOccupied)
                return false;

            state.IsOccupied = true;
            state.OccupyingUnit = unit;
            cells[cell] = state;

            // Update visual
            if (gridView != null)
            {
                var cellView = gridView.GetCellView(cell);
                if (cellView != null)
                {
                    cellView.SetOccupied(true);
                }
            }

            return true;
        }

        /// <summary>
        /// Frees a cell
        /// </summary>
        public void FreeCell(Vector2Int cell)
        {
            if (!cells.ContainsKey(cell))
                return;

            var state = cells[cell];
            state.IsOccupied = false;
            state.OccupyingUnit = null;
            cells[cell] = state;

            // Update visual
            if (gridView != null)
            {
                var cellView = gridView.GetCellView(cell);
                if (cellView != null)
                {
                    cellView.SetOccupied(false);
                }
            }
        }

        /// <summary>
        /// Finds the first available (unoccupied) cell
        /// </summary>
        public bool TryGetFirstAvailableCell(out Vector2Int cell)
        {
            foreach (var kvp in cells)
            {
                if (!kvp.Value.IsOccupied)
                {
                    cell = kvp.Key;
                    return true;
                }
            }

            cell = Vector2Int.zero;
            return false;
        }

        /// <summary>
        /// Gets the world position of a cell
        /// </summary>
        public Vector3 GetCellWorldPosition(Vector2Int cell)
        {
            if (gridContext == null)
                return Vector3.zero;

            return gridContext.GetWorldPos(cell);
        }

        /// <summary>
        /// Tries to get cell from world position
        /// </summary>
        public bool TryGetCellFromWorld(Vector3 worldPos, out Vector2Int cell)
        {
            if (gridView != null)
            {
                return gridView.TryGetCell(worldPos, out cell);
            }

            cell = Vector2Int.zero;
            return false;
        }
    }
}
