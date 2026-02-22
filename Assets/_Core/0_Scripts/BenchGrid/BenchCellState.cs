using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Runtime details of a bench cell, such as whether it's occupied, what unit is on it, etc.
    /// </summary>
    public struct BenchCellState
    {
        public bool IsOccupied;
        public GameObject OccupyingUnit;
        public MonoBehaviour OccupyingChampion; // Reference to Champion component (MonoBehaviour for now)

        // Can add more properties like:
        // public int Cost;
        // public bool IsLocked;
    }
}
