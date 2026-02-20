
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VTLTools;

namespace DucDevGame
{
    public class HexCellView : MonoBehaviour
    {
        private Renderer _renderer;
        public Renderer ThisRenderer => _renderer ?? (_renderer = GetComponentInChildren<Renderer>());

        public void Init()
        {
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

    }
}