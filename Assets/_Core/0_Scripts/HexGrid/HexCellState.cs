namespace DucDevGame
{
    /// <summary>
    /// runtime details of a cell, such as whether it's walkable, movement cost, etc.
    /// </summary>
    public struct CellState
    {
        public bool Walkable;
        public IGridEntity OccupiedBy;
    }
}