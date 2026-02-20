using System.Collections.Generic;
using UnityEngine;
using VTLTools;

namespace DucDevGame
{
    public class HexGridView : MonoBehaviour
    {
        [SerializeField] private HexGridContext gridContext;
        [SerializeField] private bool showCoordinates = false;
        [Header("Cell View Spawn")]
        [SerializeField] private HexCellView cellViewPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private bool spawnOnStart = true;

        private readonly List<HexCellView> spawnedCells = new List<HexCellView>();
        public Dictionary<Vector3Int, HexCellView> viewDic = new();

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

            HexGridData gridData = gridContext.GridData;
            for (int row = 0; row < gridData.height; row++)
            {
                for (int col = 0; col < gridData.width; col++)
                {
                    Vector3Int cube = HexMath.OffsetToCube(col, row);
                    Vector3 pos = CubeToWorldInternal(cube.x, cube.y, cube.z);
                    HexCellView cellView = ObjectPool.Spawn(cellViewPrefab);
                    cellView.transform.position = pos;
                    cellView.transform.SetParent(cellParent);
                    cellView.Init();
                    spawnedCells.Add(cellView);
                    viewDic[cube] = cellView;
                }
            }
        }

        public void ClearSpawnedCells()
        {
            for (int i = spawnedCells.Count - 1; i >= 0; i--)
            {
                HexCellView cellView = spawnedCells[i];
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
        }

        /// <summary>
        /// Converts cube coordinates to world position.
        /// </summary>
        public Vector3 CubeToWorldInternal(int cubeX, int cubeY, int cubeZ)
        {
            Vector2Int offset = HexMath.CubeToOffset(cubeX, cubeY, cubeZ);
            int col = offset.x;
            int row = offset.y;
            float rowOffset = (row & 1) == 0 ? gridContext.GetColStep() * 0.5f : 0f;
            float x = col * gridContext.GetColStep() + rowOffset;
            float z = row * gridContext.GetRowStep();

            return gridContext.GridTransform.position + gridContext.GetCenterOffset() + new Vector3(x, 0f, z);
        }

        /// <summary>
        /// Converts world position to cube coordinates (no validation, raw result).
        /// </summary>
        public Vector3Int WorldToCubeInternal(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - gridContext.GridTransform.position - gridContext.GetCenterOffset();
            int row = Mathf.RoundToInt(localPos.z / gridContext.GetRowStep());
            float rowOffset = (row & 1) == 0 ? gridContext.GetColStep() * 0.5f : 0f;
            int col = Mathf.RoundToInt((localPos.x - rowOffset) / gridContext.GetColStep());

            Vector3Int cube = HexMath.OffsetToCube(col, row);
            return cube;
        }

        /// <summary>
        /// Tries to get cube coordinates from world position, with grid bounds validation.
        /// </summary>
        public bool TryGetCube(Vector3 worldPos, out Vector3Int cube)
        {
            cube = WorldToCubeInternal(worldPos);
            return gridContext.GridData != null && Board.Instance.gridModel.IsInsideGrid(cube);
        }

        public HexCellView GetHexCellView(Vector3Int cube)
        {
            if (viewDic.TryGetValue(cube, out HexCellView cellView))
            {
                return cellView;
            }
            return null;
        }

        //===================================================
        #region [Gizmos]
        private void OnDrawGizmos()
        {
            if (gridContext == null || gridContext.GridData == null) return;

            gridContext.CacheOffsets();
            Vector3 origin = gridContext.GridTransform.position;
            Vector3 centerOffset = gridContext.GetCenterOffset();
            //Debug.Log($"<color=green>[DA]</color> {origin}, {centerOffset}");
            float colStep = gridContext.GetColStep();
            float rowStep = gridContext.GetRowStep();
            HexGridData gridData = gridContext.GridData;

            Gizmos.color = Color.gray;

            for (int row = 0; row < gridData.height; row++)
            {
                for (int col = 0; col < gridData.width; col++)
                {
                    float rowOffset = (row & 1) == 0 ? colStep * 0.5f : 0f;
                    Vector3 pos = new Vector3(col * colStep + rowOffset, 0f, row * rowStep)
                        + centerOffset + origin;

                    DrawHexagon(pos, gridData.size);
                    if (showCoordinates)
                    {
                        Vector3Int cube = HexMath.OffsetToCube(col, row);
                        DrawCoordinates(pos, cube, col, row);
                    }
                }
            }
            //DebugOrientation();
        }

        private void DrawHexagon(Vector3 center, float size)
        {
            float angleOffset = -Mathf.PI / 6f;
            //float angleOffset = 0;

            for (int i = 0; i < 6; i++)
            {
                float angle1 = Mathf.PI / 3f * i + angleOffset;
                float angle2 = Mathf.PI / 3f * (i + 1) + angleOffset;

                Vector3 p1 = center + new Vector3(size * Mathf.Cos(angle1), 0f, size * Mathf.Sin(angle1));
                Vector3 p2 = center + new Vector3(size * Mathf.Cos(angle2), 0f, size * Mathf.Sin(angle2));

                Gizmos.DrawLine(p1, p2);

            }
        }

        private void DrawCoordinates(Vector3 pos, Vector3Int cube, int col, int row)
        {
            string coordText = $"({cube.x}, {cube.y}, {cube.z})";

#if UNITY_EDITOR
            // Store current color, draw label cho từng component với màu khác
            Color xColor = Color.red;
            Color yColor = Color.green;
            Color zColor = Color.blue;

            GUI.color = xColor;
            UnityEditor.Handles.Label(pos + Vector3.up * 0.1f + Vector3.left * 0.3f + Vector3.forward * 0.3f, $"{cube.x}");

            GUI.color = yColor;
            UnityEditor.Handles.Label(pos + Vector3.up * 0.1f + Vector3.left * 0.3f + Vector3.forward * -0.35f, $"{cube.y}");

            GUI.color = zColor;
            UnityEditor.Handles.Label(pos + Vector3.up * 0.1f + Vector3.right * 0.3f, $"{cube.z}");

            GUI.color = Color.white; // reset
#endif

            // Also draw with text mesh (runtime compatible fallback)
            Debug.DrawRay(pos, Vector3.up * 0.05f, Color.yellow);
        }

        #endregion
        //===================================================

    }
}