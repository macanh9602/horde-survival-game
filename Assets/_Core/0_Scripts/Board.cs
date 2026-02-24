using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VTLTools;

namespace DucDevGame
{
    public class Board : Singleton<Board>
    {
        public HexGridModel gridModel;
        public HexPathFinder pathFinder;
        [SerializeField] private HexGridContext gridContext;
        [SerializeField] private HexGridView gridView;
        private Dictionary<Vector3Int, CellState> cells;
        private Func<Vector3Int, CellState> cachedCellDetails;
        public HexGridView GridView => gridView;
        private Dictionary<Vector3Int, List<IGridEntity>> _entitiesAtCell = new();

        private void Start()
        {
            gridModel = new HexGridModel(gridContext.GridData.width, gridContext.GridData.height);
            pathFinder = new HexPathFinder(gridModel);
            gridContext.CacheOffsets();
            cells = new Dictionary<Vector3Int, CellState>();
            foreach (var cell in gridModel.allCells)
            {
                cells[cell] = new CellState { Walkable = true };
            }
            cachedCellDetails = GetCellDetail;
        }

        /// <summary>
        /// Gets cell details using Cube coordinates.
        /// </summary>
        public CellState GetCellDetail(Vector3Int cube)
        {
            if (!cells.TryGetValue(cube, out var cell))
                return new CellState { Walkable = false };

            return new CellState
            {
                Walkable = cell.Walkable
            };
        }


        public void RegisterEntity(Vector3Int pos, IGridEntity entity)
        {
            if (!_entitiesAtCell.ContainsKey(pos))
                _entitiesAtCell[pos] = new List<IGridEntity>();

            _entitiesAtCell[pos].Add(entity);

        }

        public T GetEntityAt<T>(Vector3Int pos) where T : IGridEntity
        {
            if (_entitiesAtCell.TryGetValue(pos, out var list))
            {
                return list.OfType<T>().FirstOrDefault();
            }
            return default;
        }

        [Button("Test Pathfinding")]
        public void TestPathfinding(BaseChampionBehavior champ, Vector3Int end)
        {
            DebugUtils.DrawWireSphere(GridView.CubeToWorldInternal(champ.HexGridPos.x, champ.HexGridPos.y, champ.HexGridPos.z), 0.5f, Color.green, 2f);
            var start = champ.HexGridPos;
            var path = pathFinder.FindPath(start, end, cachedCellDetails);
            foreach (var cellCube in path)
            {
                DebugUtils.DrawWireSphere(gridContext.GetWorldPos(cellCube), 0.2f, Color.blue, 2f);
            }
            List<Vector3> worldPath = new List<Vector3>();
            for (int i = 0; i < path.Count; i++)
            {
                worldPath.Add(gridContext.GetWorldPos(path[i]));
            }
            champ.MoveTo(worldPath);

        }

    }
}
