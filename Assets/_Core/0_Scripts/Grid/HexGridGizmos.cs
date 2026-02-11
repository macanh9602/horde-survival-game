using UnityEngine;

public class HexGridGizmos : MonoBehaviour
{
    [SerializeField] private HexGridContext gridContext;
    [SerializeField] private bool showCoordinates = false;

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

        Gizmos.color = Color.green;

        for (int row = 0; row < gridData.height; row++)
        {
            for (int col = 0; col < gridData.width; col++)
            {
                float rowOffset = (row & 1) == 0 ? 0f : colStep * -0.5f;
                Vector3 pos = new Vector3(col * colStep + rowOffset, 0f, row * rowStep)
                    + centerOffset + origin;

                DrawHexagon(pos, gridData.size);
                if (showCoordinates)
                {
                    Vector2Int axial = new Vector2Int((int)HexMath.OffsetToAxial(col, row).x, (int)HexMath.OffsetToAxial(col, row).y);
                    DrawCoordinates(pos, axial, col, row);
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

    private void DrawCoordinates(Vector3 pos, Vector2Int axial, int col, int row)
    {
        string coordText = $"({axial.x}, {axial.y})";

#if UNITY_EDITOR
        UnityEditor.Handles.Label(pos + Vector3.up * 0.1f + Vector3.left * 0.4f, coordText);
#endif

        // Also draw with text mesh (runtime compatible fallback)
        Debug.DrawRay(pos, Vector3.up * 0.05f, Color.yellow);
    }

    private string GetCubeCoords(Vector2Int axial)
    {
        // Convert axial (q, r) to cube (x, y, z) where x+y+z=0
        int q = axial.x;
        int r = axial.y;
        int x = q;
        int z = r;
        int y = -x - z;
        return $"({x}, {y}, {z})";
    }
}