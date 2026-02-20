using System.Collections.Generic;
using UnityEngine;
using VTLTools;

namespace DucDevGame
{
    public class BenchGridView : MonoBehaviour
    {
        [SerializeField] private BenchGridContext gridContext;
        [SerializeField] private bool showCoordinates = false;
        [Header("Cell View Spawn")]
        [SerializeField] private BenchCellView cellViewPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private bool spawnOnStart = true;

        private readonly List<BenchCellView> spawnedCells = new List<BenchCellView>();
        public Dictionary<Vector2Int, BenchCellView> viewDic = new();

        private void Start()
        {
            if (spawnOnStart)
            {
                SpawnCells();
            }
        }

        public void SpawnCells()
        {
            if (gridContext == null || gridContext.GridData == null || cellViewPrefab == null)
            {
                return;
            }

            gridContext.CacheOffsets();
            ClearSpawnedCells();

            BenchGridData gridData = gridContext.GridData;
            for (int row = 0; row < gridData.height; row++)
            {
                for (int col = 0; col < gridData.width; col++)
                {
                    Vector2Int cell = new Vector2Int(col, row);
                    Vector3 pos = gridContext.GetWorldPos(cell);

                    BenchCellView cellView = ObjectPool.Spawn(cellViewPrefab);
                    cellView.transform.position = pos;
                    cellView.transform.SetParent(cellParent);
                    cellView.Init(cell);

                    spawnedCells.Add(cellView);
                    viewDic[cell] = cellView;
                }
            }
        }

        public void ClearSpawnedCells()
        {
            for (int i = spawnedCells.Count - 1; i >= 0; i--)
            {
                BenchCellView cellView = spawnedCells[i];
                if (cellView == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(cellView.gameObject);
                }
                else
                {
                    DestroyImmediate(cellView.gameObject);
                }
            }

            spawnedCells.Clear();
            viewDic.Clear();
        }

        /// <summary>
        /// Converts world position to grid coordinates (no validation)
        /// </summary>
        public Vector2Int WorldToCell(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - gridContext.GridTransform.position - gridContext.GetCenterOffset();

            int col = Mathf.RoundToInt(localPos.x / gridContext.GetCellStepX());
            int row = Mathf.RoundToInt(localPos.z / gridContext.GetCellStepZ());

            return new Vector2Int(col, row);
        }

        /// <summary>
        /// Tries to get cell coordinates from world position, with bounds validation
        /// </summary>
        public bool TryGetCell(Vector3 worldPos, out Vector2Int cell)
        {
            cell = WorldToCell(worldPos);
            return viewDic.ContainsKey(cell);
        }

        public BenchCellView GetCellView(Vector2Int cell)
        {
            if (viewDic.TryGetValue(cell, out BenchCellView cellView))
            {
                return cellView;
            }
            return null;
        }

        public BenchCellView GetCellView(int col, int row)
        {
            return GetCellView(new Vector2Int(col, row));
        }

        //===================================================
        #region [Gizmos]
        private void OnDrawGizmos()
        {
            if (gridContext == null || gridContext.GridData == null) return;

            gridContext.CacheOffsets();
            Vector3 origin = gridContext.GridTransform.position;
            Vector3 centerOffset = gridContext.GetCenterOffset();
            float cellStepX = gridContext.GetCellStepX();
            float cellStepZ = gridContext.GetCellStepZ();
            BenchGridData gridData = gridContext.GridData;

            Gizmos.color = Color.cyan;

            for (int row = 0; row < gridData.height; row++)
            {
                for (int col = 0; col < gridData.width; col++)
                {
                    Vector3 pos = new Vector3(col * cellStepX, 0f, row * cellStepZ)
                        + centerOffset + origin;

                    DrawRectangle(pos, gridData.cellWidth, gridData.cellHeight);

                    if (showCoordinates)
                    {
                        DrawCoordinates(pos, col, row);
                    }
                }
            }
        }

        private void DrawRectangle(Vector3 center, float width, float height)
        {
            float halfW = width * 0.5f;
            float halfH = height * 0.5f;

            Vector3 p1 = center + new Vector3(-halfW, 0f, -halfH);
            Vector3 p2 = center + new Vector3(halfW, 0f, -halfH);
            Vector3 p3 = center + new Vector3(halfW, 0f, halfH);
            Vector3 p4 = center + new Vector3(-halfW, 0f, halfH);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }

        private void DrawCoordinates(Vector3 pos, int col, int row)
        {
#if UNITY_EDITOR
            string coordText = $"({col}, {row})";
            GUI.color = Color.yellow;
            UnityEditor.Handles.Label(pos + Vector3.up * 0.1f + Vector3.right * -0.2f, coordText);
            GUI.color = Color.white;
#endif
        }

        #endregion
        //===================================================
    }
}
