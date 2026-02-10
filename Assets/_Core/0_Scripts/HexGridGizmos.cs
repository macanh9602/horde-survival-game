using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Board : MonoBehaviour
{
    private enum CoordinateMode
    {
        Cube,
        Axial,
        Doubled
    }

    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float size = 1.0f;
    [SerializeField] private float spacing = 0.0f;
    [SerializeField] private Transform parent;
    [SerializeField] private Color gizmoColor = new Color(0.2f, 0.9f, 0.6f, 0.8f);
    [SerializeField] private bool showCoordinates = false;
    [SerializeField] private CoordinateMode coordinateMode = CoordinateMode.Axial;

    public void OnDrawGizmos()
    {
        if (width <= 0 || height <= 0 || size <= 0f)
        {
            return;
        }

        Transform origin = parent != null ? parent : transform;
        float hexWidth = Mathf.Sqrt(3f) * size;
        float hexHeight = 2f * size;
        float colStep = hexWidth + spacing;
        float rowStep = 0.75f * hexHeight + spacing;
        bool hasOddRow = height > 1;
        float maxX = (width - 1) * colStep + (hasOddRow ? colStep * 0.5f : 0f);
        float maxZ = (height - 1) * rowStep;
        Vector3 centerOffset = new Vector3(-maxX * 0.5f, 0f, -maxZ * 0.5f);

        Gizmos.color = gizmoColor;

        for (int row = 0; row < height; row++)
        {
            float rowOffset = (row & 1) == 0 ? 0f : colStep * 0.5f;
            float z = row * rowStep;

            for (int col = 0; col < width; col++)
            {
                float x = col * colStep + rowOffset;
                Vector3 center = origin.position + centerOffset + new Vector3(x, 0f, z);
                DrawHex(center, size);

#if UNITY_EDITOR
                if (showCoordinates)
                {
                    string label = FormatCoordinates(col, row);
                    Handles.Label(center, label);
                }
#endif
            }
        }
    }

    private static void DrawHex(Vector3 center, float size)
    {
        Vector3[] points = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            float angleDeg = 60f * i + 30f;
            float angleRad = angleDeg * Mathf.Deg2Rad;
            points[i] = center + new Vector3(Mathf.Cos(angleRad) * size, 0f, Mathf.Sin(angleRad) * size);
        }

        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
        }
    }

    private string FormatCoordinates(int col, int row)
    {
        int q = col - (row - (row & 1)) / 2;
        int r = row;

        switch (coordinateMode)
        {
            case CoordinateMode.Cube:
                int x = q;
                int z = r;
                int y = -x - z;
                return $"({x},{y},{z})";
            case CoordinateMode.Doubled:
                int dx = 2 * col + (row & 1);
                int dy = row;
                return $"({dx},{dy})";
            default:
                return $"({q},{r})";
        }
    }
}
