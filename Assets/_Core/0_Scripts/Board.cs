using UnityEngine;
using VTLTools;
using System.Collections.Generic;

public class Board : Singleton<Board>
{
    [SerializeField] private HexGridContext gridContext;
    // [SerializeField] private List<Entity> entities = new List<Entity>();

    public HexGridContext GridContext => gridContext;
    // public IReadOnlyList<Entity> Entities => entities;

    // public void AddEntity(Entity entity)
    // {
    //     entities.Add(entity);
    // }

    // public void RemoveEntity(Entity entity)
    // {
    //     entities.Remove(entity);
    // }

    // Legacy properties for backward compatibility - will be removed
    public HexGridData gridData => gridContext != null ? gridContext.GridData : null;
    public Transform gridTransform => gridContext != null ? gridContext.GridTransform : null;

    public Vector3 GetCenterOffset()
    {
        return gridContext != null ? gridContext.GetCenterOffset() : Vector3.zero;
    }
}