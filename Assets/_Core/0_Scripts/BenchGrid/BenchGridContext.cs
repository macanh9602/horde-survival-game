using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Holds bench grid configuration and runtime context
    /// </summary>
    public class BenchGridContext : MonoBehaviour
    {
        [SerializeField] private BenchGridData gridData;

        public BenchGridData GridData => gridData;
        public Transform GridTransform => transform;

        private Vector3 _centerOffset;
        private float _cellStepX;
        private float _cellStepZ;

        public void CacheOffsets()
        {
            if (gridData == null || gridData.width <= 0 || gridData.height <= 0)
            {
                return;
            }

            _cellStepX = gridData.cellWidth + gridData.spacing;
            _cellStepZ = gridData.cellHeight + gridData.spacing;

            // Calculate center offset to center the grid
            float totalWidth = (gridData.width - 1) * _cellStepX;
            float totalHeight = (gridData.height - 1) * _cellStepZ;

            _centerOffset = new Vector3(-totalWidth * 0.5f, 0f, -totalHeight * 0.5f);
        }

        public Vector3 GetCenterOffset() => _centerOffset;
        public float GetCellStepX() => _cellStepX;
        public float GetCellStepZ() => _cellStepZ;

        /// <summary>
        /// Converts grid coordinates to world position
        /// </summary>
        public Vector3 GetWorldPos(Vector2Int cell)
        {
            return GetWorldPos(cell.x, cell.y);
        }

        /// <summary>
        /// Converts grid coordinates to world position
        /// </summary>
        public Vector3 GetWorldPos(int col, int row)
        {
            float x = col * _cellStepX;
            float z = row * _cellStepZ;

            return GridTransform.position + _centerOffset + new Vector3(x, 0f, z);
        }
    }
}
