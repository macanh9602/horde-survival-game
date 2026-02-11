using UnityEngine;

/// <summary>
/// Holds hex grid configuration and runtime context.
/// Decouples grid logic from Board singleton.
/// </summary>
public class HexGridContext : MonoBehaviour
{
    [SerializeField] private HexGridData gridData;

    public HexGridData GridData => gridData;
    public Transform GridTransform => transform;

    private Vector3 _centerOffset;
    private float _colStep;
    private float _rowStep;

    private void OnEnable()
    {
        CacheOffsets();
    }

    public void CacheOffsets()
    {
        if (gridData == null || gridData.width <= 0 || gridData.height <= 0 || gridData.size <= 0f)
        {
            return;
        }

        float hexHeight = 2f * gridData.size;
        float hexWidth = Mathf.Sqrt(3f) * gridData.size;
        _colStep = hexWidth + gridData.spacing;
        _rowStep = 0.75f * hexHeight + gridData.spacing;

        bool hasOddRow = gridData.height > 1;
        float maxX = (gridData.width - 1) * _colStep + (hasOddRow ? _colStep * 0.5f : 0f);
        float maxZ = (gridData.height - 1) * _rowStep;

        _centerOffset = new Vector3(-maxX * 0.5f, 0f, -maxZ * 0.5f);
    }

    public Vector3 GetCenterOffset() => _centerOffset;
    public float GetColStep() => _colStep;
    public float GetRowStep() => _rowStep;

    public Vector3 GetWorldPos(int axialX, int axialY)
    {
        Vector2 offset = HexMath.AxialToOffset(axialX, axialY);
        int row = (int)offset.y;
        float rowOffset = (row & 1) == 0 ? 0f : _colStep * -0.5f;
        float x = offset.x * _colStep + rowOffset;
        float z = offset.y * _rowStep;

        return GridTransform.position + _centerOffset + new Vector3(x, 0f, z);
    }


}
