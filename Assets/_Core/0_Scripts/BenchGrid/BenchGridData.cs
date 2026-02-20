namespace DucDevGame
{
    [System.Serializable]
    public class BenchGridData
    {
        public int width = 8;  // 8 columns fixed
        public int height = 1; // 1 row fixed
        public float cellWidth = 1f;
        public float cellHeight = 1f;
        public float spacing = 0.1f;
    }
}
