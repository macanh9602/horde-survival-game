using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Converts between world and axial coordinates.
/// Bridge between HexGridContext (data) and HexMath (algorithms).
/// Handles: transformation, validation, querying.
/// </summary>
public class HexGridConverter : MonoBehaviour
{
    [SerializeField] private HexGridContext gridContext;

    #region Transformation Logic (private utilities)

    /// <summary>
    /// Calculates colStep, rowStep, and centerOffset from grid configuration.
    /// </summary>
    private static (float colStep, float rowStep, Vector3 centerOffset) CalcGridOffsets(HexGridData gridData)
    {
        if (gridData == null || gridData.width <= 0 || gridData.height <= 0 || gridData.size <= 0f)
        {
            return (0f, 0f, Vector3.zero);
        }

        float hexHeight = 2f * gridData.size;
        float hexWidth = Mathf.Sqrt(3f) * gridData.size;
        float colStep = hexWidth + gridData.spacing;
        float rowStep = 0.75f * hexHeight + gridData.spacing;

        bool hasOddRow = gridData.height > 1;
        float maxX = (gridData.width - 1) * colStep + (hasOddRow ? colStep * 0.5f : 0f);
        float maxZ = (gridData.height - 1) * rowStep;

        Vector3 centerOffset = new Vector3(-maxX * 0.5f, 0f, -maxZ * 0.5f);
        return (colStep, rowStep, centerOffset);
    }

    /// <summary>
    /// Internal: converts axial to world (no validation).
    /// </summary>
    private Vector3 AxialToWorldInternal(int axialX, int axialY)
    {
        Vector2 offset = HexMath.AxialToOffset(axialX, axialY);
        int row = (int)offset.y;
        float rowOffset = (row & 1) == 0 ? 0f : gridContext.GetColStep() * -0.5f;
        float x = offset.x * gridContext.GetColStep() + rowOffset;
        float z = offset.y * gridContext.GetRowStep();

        return gridContext.GridTransform.position + gridContext.GetCenterOffset() + new Vector3(x, 0f, z);
    }

    /// <summary>
    /// Internal: converts world to axial (no validation, raw result).
    /// </summary>
    private Vector2Int WorldToAxialInternal(Vector3 worldPos)
    {
        Vector3 localPos = worldPos - gridContext.GridTransform.position - gridContext.GetCenterOffset();
        int row = Mathf.RoundToInt(localPos.z / gridContext.GetRowStep());
        float rowOffset = (row & 1) == 0 ? 0f : gridContext.GetColStep() * -0.5f;
        int col = Mathf.RoundToInt((localPos.x - rowOffset) / gridContext.GetColStep());

        Vector2 axialFloat = HexMath.OffsetToAxial(col, row);
        Vector2 rounded = HexMath.HexRound(axialFloat.x, axialFloat.y);
        return new Vector2Int((int)rounded.x, (int)rounded.y);
    }

    #endregion

    #region Public API (Safe - with validation)

    /// <summary>
    /// Converts world position to axial, validates bounds.
    /// </summary>
    public bool TryGetAxial(Vector3 worldPos, out Vector2Int axial)
    {
        axial = default;

        if (gridContext == null || gridContext.GridData == null)
            return false;

        axial = WorldToAxialInternal(worldPos);
        return IsInsideGrid(axial);
    }

    /// <summary>
    /// Converts axial to world position if inside grid.
    /// </summary>
    public bool TryGetWorld(Vector2Int axial, out Vector3 worldPos)
    {
        worldPos = Vector3.zero;

        if (!IsInsideGrid(axial))
            return false;

        worldPos = AxialToWorldInternal(axial.x, axial.y);
        return true;
    }

    /// <summary>
    /// Checks if axial coordinates are within grid bounds.
    /// </summary>
    public bool IsInsideGrid(Vector2Int axial)
    {
        if (gridContext == null || gridContext.GridData == null)
            return false;

        Vector2 offset = HexMath.AxialToOffset(axial.x, axial.y);
        int col = (int)offset.x;
        int row = (int)offset.y;

        return col >= 0 && col < gridContext.GridData.width &&
               row >= 0 && row < gridContext.GridData.height;
    }

    /// <summary>
    /// Returns all 6 neighbors if they're inside grid.
    /// </summary>
    public List<Vector2Int> GetNeighbors(Vector2Int axial)
    {
        var neighbors = new List<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
        };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = new Vector2Int(axial.x + dir.x, axial.y + dir.y);
            if (IsInsideGrid(neighbor))
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    #endregion
}