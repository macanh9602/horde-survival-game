using UnityEngine;
using VTLTools;

public class InputHandle : MonoBehaviour
{
    // [SerializeField] private LayerMask groundLayer;
    [SerializeField] private HexGridConverter gridConverter;
    [SerializeField] private TacticianBase currentTactician;
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
                if (gridConverter.TryGetAxial(hitInfo.point, out Vector2Int axial))
                {
                    if (gridConverter.TryGetWorld(axial, out Vector3 cellCenter))
                    {
                        DebugUtils.DrawWireSphere(cellCenter, 0.2f, Color.red, 2f);
                        OnHexClicked(axial);
                    }
                }
                cachedMouseWorldPos = hitInfo.point;
                currentTactician.MoveTo(cachedMouseWorldPos);
            }
        }
    }

    private void OnHexClicked(Vector2Int axial)
    {
        //Debug.Log($"Hex clicked: {axial}");
    }
}