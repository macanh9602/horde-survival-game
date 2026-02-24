using System;
using UnityEngine;

namespace DucDevGame
{
    public interface IGridEntity
    {
        public Vector3Int HexGridPos { get; set; }
        // int EntityID { get; }
        public EntityType Type { get; }
        public Team Team { get; }

    }

    public enum EntityType
    {
        Champion,
    }

    public enum Team
    {
        Neutral = 0,
        Player1 = 1,
        Player2 = 2
    }
}