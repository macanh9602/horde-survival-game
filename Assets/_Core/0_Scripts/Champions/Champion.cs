using UnityEngine;

namespace DucDevGame
{
    /// <summary>
    /// Base class for all champion units
    /// </summary>
    public class Champion : MonoBehaviour
    {
        #region Fields
        private ChampionConfig config;

        // Components
        private ChampionStatRuntime stat;
        private ChampionGraphic graphic;
        private ChampionBehavior behavior;

        // Grid data
        private Vector2Int currentCell;
        private bool isOnBench;

        #endregion

        #region Properties
        public ChampionConfig Config => config;
        public ChampionName ChampionType => config.type;
        public bool IsAlive => stat != null && stat.GetCurrentHP() > 0;
        public Vector2Int CurrentCell => currentCell;
        public bool IsOnBench => isOnBench;
        public ChampionStatRuntime Stat => stat;
        public ChampionGraphic Graphic => graphic;
        public ChampionBehavior Behavior => behavior;
        #endregion

        #region Lifecycle
        private void Awake()
        {
            // Get components
            stat = GetComponent<ChampionStatRuntime>();
            graphic = GetComponent<ChampionGraphic>();
            behavior = GetComponent<ChampionBehavior>();

            // Add missing components
            if (stat == null) stat = gameObject.AddComponent<ChampionStatRuntime>();
            if (graphic == null) graphic = gameObject.AddComponent<ChampionGraphic>();
            if (behavior == null) behavior = gameObject.AddComponent<ChampionBehavior>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize champion with config and stats
        /// </summary>
        public void Initialize(ChampionConfig championConfig, Vector2Int cell, int level, int stars, bool onBench)
        {
            config = championConfig;
            currentCell = cell;
            isOnBench = onBench;

            // Initialize stat component
            if (stat != null && stat.baseStat != null)
            {
                // Apply level/star bonuses to baseStat
                stat.baseStat.baseHP += level * 10f + stars * 20f;
                stat.baseStat.baseDamage += level * 2f + stars * 5f;
                stat.baseStat.baseArmor += level * 0.5f;
            }

            // Initialize other components
            if (behavior != null)
            {
                behavior.SetStat(stat);
                behavior.SetGraphic(config.GraphicsPrefab);
            }

            gameObject.name = $"{config.type}_Lv{level}_Star{stars}";
        }

        /// <summary>
        /// Update cell position when moved
        /// </summary>
        public void UpdateCell(Vector2Int newCell, bool onBench)
        {
            currentCell = newCell;
            isOnBench = onBench;
        }

        /// <summary>
        /// Take damage
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (stat == null) return;

            float armor = stat.GetCurrentArmor();
            float actualDamage = Mathf.Max(1f, damage - armor);

            // TODO: Implement health change logic
            // stat.TakeDamage(actualDamage);
        }

        /// <summary>
        /// Attack target
        /// </summary>
        public void Attack(Champion target)
        {
            if (behavior == null || target == null) return;

            behavior.SetStat(stat);
            // TODO: Implement attack logic
        }

        /// <summary>
        /// Play animation
        /// </summary>
        public void PlayAnimation(string animName)
        {
            if (graphic == null) return;
            // TODO: Implement animation logic
        }
        #endregion
    }
}
