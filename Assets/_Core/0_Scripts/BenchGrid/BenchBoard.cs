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

        private Dictionary<Vector2Int, BenchCellState> cellsMapping;

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
            cellsMapping = new Dictionary<Vector2Int, BenchCellState>();
            foreach (var cell in gridModel.allCells)
            {
                cellsMapping[cell] = new BenchCellState
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
            if (!cellsMapping.TryGetValue(cell, out var state))
            {
                return new BenchCellState { IsOccupied = true }; // Invalid cells are considered occupied
            }
            return state;
        }

        /// <summary>
        /// Sets a cell as occupied by a unit
        /// </summary>
        public bool OccupyCell(Vector2Int cell, GameObject unit, MonoBehaviour champion = null)
        {
            if (!cellsMapping.ContainsKey(cell))
                return false;

            var state = cellsMapping[cell];
            if (state.IsOccupied)
                return false;

            state.IsOccupied = true;
            state.OccupyingUnit = unit;
            state.OccupyingChampion = champion;
            cellsMapping[cell] = state;

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
            if (!cellsMapping.ContainsKey(cell))
                return;

            var state = cellsMapping[cell];
            state.IsOccupied = false;
            state.OccupyingUnit = null;
            state.OccupyingChampion = null;
            cellsMapping[cell] = state;

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
            foreach (var kvp in cellsMapping)
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

        /// <summary>
        /// Spawns a champion to a specific bench cell (only if IsOccupied = false)
        /// </summary>
        public Champion SpawnChampionToCell(ChampionName championType, Vector2Int cell, int level = 1, int stars = 1)
        {
            // Check if cell is valid and unoccupied
            if (!cellsMapping.ContainsKey(cell))
            {
                Debug.LogWarning($"Invalid bench cell: {cell}");
                return null;
            }

            var state = GetCellState(cell);
            if (state.IsOccupied)
            {
                Debug.LogWarning($"Bench cell {cell} is already occupied!");
                return null;
            }

            // Get champion config
            ChampionConfig config = ChampionDatabases.GetData(championType);
            if (config == null)
            {
                Debug.LogError($"Champion config not found: {championType}");
                return null;
            }

            // Get world position for this cell
            Vector3 worldPos = GetCellWorldPosition(cell);

            // Spawn graphics prefab
            GameObject championObj = Object.Instantiate(config.GraphicsPrefab);
            championObj.transform.position = worldPos;
            championObj.transform.SetParent(gridContext.GridTransform);

            // Get or add Champion component
            Champion champion = championObj.GetComponent<Champion>();
            if (champion == null)
            {
                champion = championObj.AddComponent<Champion>();
            }

            // Initialize champion
            champion.Initialize(config, cell, level, stars, true);

            // Occupy cell
            OccupyCell(cell, championObj, champion as MonoBehaviour);

            Debug.Log($"Spawned {championType} to bench cell {cell} at position {worldPos}");
            return champion;
        }

        /// <summary>
        /// Spawns a champion to the first available bench cell
        /// </summary>
        public Champion SpawnChampionToFirstAvailableCell(ChampionName championType, int level = 1, int stars = 1)
        {
            if (TryGetFirstAvailableCell(out Vector2Int cell))
            {
                return SpawnChampionToCell(championType, cell, level, stars);
            }

            Debug.LogWarning("No available bench cells!");
            return null;
        }

        /// <summary>
        /// Removes champion from bench cell
        /// </summary>
        public void RemoveChampionFromCell(Vector2Int cell)
        {
            if (!cellsMapping.ContainsKey(cell))
                return;

            var state = GetCellState(cell);
            if (state.OccupyingUnit != null)
            {
                Object.Destroy(state.OccupyingUnit);
            }

            FreeCell(cell);
        }
    }
}
