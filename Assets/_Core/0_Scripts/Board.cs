using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VTLTools;

public class Board : Singleton<Board>
{
    public HexGridModel gridModel;
    public HexPathFinder pathFinder;
    [SerializeField] private HexGridContext gridContext;
    private Dictionary<Vector3Int, CellDetail> cells;
    private Func<Vector3Int, CellDetail> cachedCellDetails;

    private void Start()
    {
        gridModel = new HexGridModel(gridContext.GridData.width, gridContext.GridData.height);
        pathFinder = new HexPathFinder(gridModel);
        gridContext.CacheOffsets();
        cells = new Dictionary<Vector3Int, CellDetail>();
        foreach (var cell in gridModel.allCells)
        {
            cells[cell] = new CellDetail { Walkable = true };
        }
        cachedCellDetails = GetCellDetail;
    }

    /// <summary>
    /// Gets cell details using Cube coordinates.
    /// </summary>
    public CellDetail GetCellDetail(Vector3Int cube)
    {
        if (!cells.TryGetValue(cube, out var cell))
            return new CellDetail { Walkable = false };

        return new CellDetail
        {
            Walkable = cell.Walkable
        };
    }

    [Button("Test Pathfinding")]
    public void TestPathfinding(Vector3Int start, Vector3Int end)
    {
        var path = pathFinder.FindPath(start, end, cachedCellDetails);
        foreach (var cellCube in path)
        {
            DebugUtils.DrawWireSphere(gridContext.GetWorldPos(cellCube), 0.2f, Color.blue, 2f);
        }
    }

}
