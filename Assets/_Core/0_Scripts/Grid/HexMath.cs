
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Provides mathematical conversions and calculations for hexagonal grids.
/// </summary>
public static class HexMath
{

    public static Vector2 OffsetToAxial(int col, int row)
    {
        int q = col - (row - (row & 1)) / 2;
        int r = row;
        return new Vector2(q, r);
    }

    public static Vector2 AxialToOffset(int q, int r)
    {
        int col = q + (r - (r & 1)) / 2;
        int row = r;
        return new Vector2(col, row);
    }

    public static Vector2 WorldToAxial(Vector3 worldPos, float size, Vector3 origin, Vector3 centerOffset)
    {
        // Chuyển vị trí thế giới về không gian local của lưới trước khi tính
        Vector3 localPos = worldPos - (origin + centerOffset);
        float q = (Mathf.Sqrt(3f) / 3f * localPos.x - 1f / 3f * localPos.z) / size;
        float r = (2f / 3f * localPos.z) / size;
        return new Vector2(q, r);
    }

    public static Vector3 AxialToWorld(float q, float r, Vector3 origin, Vector3 centerOffset, float size = 1, float spacing = 0f)
    {
        float x = size * Mathf.Sqrt(3f) * (q + r / 2f);
        float z = size * 1.5f * r;
        return origin + centerOffset + new Vector3(x, 0f, z);
    }

    public static Vector2 HexRound(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float xDiff = Mathf.Abs(rx - x);
        float yDiff = Mathf.Abs(ry - y);
        float zDiff = Mathf.Abs(rz - z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector2(rx, rz);
    }
}