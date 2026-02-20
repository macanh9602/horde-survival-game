using Sirenix.OdinInspector;
using UnityEngine;
using VTLTools;

namespace DucDevGame
{
    public class BenchCellView : MonoBehaviour
    {
        private Renderer _renderer;
        public Renderer ThisRenderer => _renderer ?? (_renderer = GetComponentInChildren<Renderer>());

        public Vector2Int CellCoordinate { get; private set; }

        public void Init(Vector2Int coordinate)
        {
            CellCoordinate = coordinate;
            ActiveView(true);
            ActivateHighlight(false);
        }

        [Button]
        public void ActiveView(bool isActive)
        {
            ThisRenderer.enabled = isActive;
        }

        [Button]
        public void ActivateHighlight(bool isActive)
        {
            ThisRenderer.enabled = true;
            ThisRenderer.SetEmissionSelfGlow(isActive ? 1f : 0f);
        }

        public void SetOccupied(bool occupied)
        {
            // Visual feedback for occupied cells
            if (occupied)
            {
                ThisRenderer.material.color = new Color(0.8f, 0.8f, 0.8f);
            }
            else
            {
                ThisRenderer.material.color = Color.white;
            }
        }
    }
}
