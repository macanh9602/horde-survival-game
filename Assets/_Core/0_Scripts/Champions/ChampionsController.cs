using Sirenix.OdinInspector;
using UnityEngine;
namespace DucDevGame
{
    /// <summary>
    /// Controller for spawning and managing champions in the bench grid
    /// </summary>
    public class ChampionsController : MonoBehaviour
    {
        [SerializeField] private BenchBoard benchBoard;

        private void Start()
        {
            if (benchBoard == null)
            {
                benchBoard = BenchBoard.Instance;
            }
        }

        /// <summary>
        /// Spawn champion to specific bench cell
        /// </summary>
        [Button("Test Spawn at Cell (0,0)")]
        public BaseChampionBehavior SpawnChampionToCell(ChampionName name, Vector2Int cell, int level = 1, int stars = 1)
        {
            if (benchBoard == null)
            {
                Debug.LogError("BenchBoard not assigned!");
                return null;
            }

            return benchBoard.SpawnChampionToCell(name, cell, level, stars);
        }

        /// <summary>
        /// Spawn champion to first available bench cell
        /// </summary>
        [Button("Test Spawn to Available Cell")]
        public BaseChampionBehavior SpawnChampionToAvailableCell(ChampionName name = ChampionName.Garen, int level = 1, int stars = 1)
        {
            if (benchBoard == null)
            {
                Debug.LogError("BenchBoard not assigned!");
                return null;
            }

            return benchBoard.SpawnChampionToFirstAvailableCell(name, level, stars);
        }

        /// <summary>
        /// Remove champion from bench
        /// </summary>
        public void RemoveChampion(BaseChampionBehavior champion)
        {
            if (benchBoard == null || champion == null) return;

            benchBoard.RemoveChampionFromCell(champion.CurrentCell);
        }

        /// <summary>
        /// Get champion at specific cell
        /// </summary>
        public BaseChampionBehavior GetChampionAtCell(Vector2Int cell)
        {
            if (benchBoard == null) return null;

            var state = benchBoard.GetCellState(cell);
            return state.OccupyingChampion as BaseChampionBehavior;
        }
    }
}
