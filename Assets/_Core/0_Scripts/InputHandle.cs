using UnityEngine;
using VTLTools;
using VTLTools.Effect;

namespace DucDevGame
{
    public class InputHandle : MonoBehaviour
    {
        // [SerializeField] private LayerMask groundLayer;
        [SerializeField] private TacticianBase currentTactician;
        [SerializeField] private HexGridView gridView;
        [SerializeField] private Effect clickEffectPrefab;
        public Vector3 cachedMouseWorldPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask groundLayer = LayerMask.GetMask(StaticVariables.Ground_Layer);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.yellow, 2f);
                    if (gridView.TryGetCube(hitInfo.point, out Vector3Int cube))
                    {
                        Vector3 cellCenter = gridView.CubeToWorldInternal(cube.x, cube.y, cube.z);
                        DebugUtils.DrawWireSphere(cellCenter, 0.2f, Color.red, 2f);
                        OnHexClicked(cube);
                    }
                    cachedMouseWorldPos = hitInfo.point;
                    currentTactician.MoveTo(cachedMouseWorldPos);
                    Effect effect = ObjectPool.Spawn(clickEffectPrefab, cachedMouseWorldPos, Quaternion.identity);
                    effect.Play();
                }
            }
        }

        private void OnHexClicked(Vector3Int cube)
        {
            //Debug.Log($"Hex clicked: {cube}");
        }
    }
}