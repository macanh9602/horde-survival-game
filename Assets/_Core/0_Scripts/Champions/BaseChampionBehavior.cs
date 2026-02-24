using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

namespace DucDevGame
{
    /// <summary>
    /// Base class for all champion units and behavior orchestration
    /// </summary>
    public class BaseChampionBehavior : MonoBehaviour, IGridEntity, IHealth
    {
        #region Fields

        private ChampionConfig config;

        // Components
        [SerializeField] private ChampionStatRuntime stat;
        [SerializeField] private ChampionMovement movement;
        [SerializeField] private Transform modelRoot;
        [SerializeField] private HealthBarBehavior healthBarBehavior;

        private ChampionGraphic currentGraphic;
        private GameObject currentGraphicsPrefab;
        private GameObject currentGraphicInstance;

        // Grid data
        private Vector2Int currentCell;
        private bool isOnBench;
        public Team team = Team.Player1;

        #endregion

        #region Properties
        public ChampionConfig Config => config;
        public ChampionName ChampionType => config.type;
        public bool IsAlive => stat != null && stat.GetCurrentHP() > 0;
        public Vector2Int CurrentCell => currentCell;
        public bool IsOnBench => isOnBench;
        public ChampionStatRuntime Stat => stat;
        public ChampionGraphic Graphic => currentGraphic;
        public BaseChampionBehavior Behavior => this;
        public ChampionMovement Movement => movement;
        public ChampionGraphic CurrentGraphic => currentGraphic;


        public Vector3Int HexGridPos { get => Board.Instance.GridView.WorldToCubeInternal(Graphic.transform.position); set { } }

        public EntityType Type => EntityType.Champion;

        public Team Team => team;


        public float MaxHealth => 100;

        public float CurrentHealth => stat != null ? stat.GetCurrentHP() : 0;
        public Action<IGridEntity, Vector3Int, Vector3Int> OnPositionChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action OnTakenDamage;

        #endregion

        #region Lifecycle
        private void Awake()
        {
            SetStatContext(stat);
        }

        private void FixedUpdate()
        {
            healthBarBehavior.FollowUpdate();
        }
        #endregion

        #region Public Methods
        public void SetStatContext(ChampionStatRuntime championStat)
        {
            stat = championStat;
        }

        public void SetGraphicPrefab(GameObject graphicsPrefab)
        {
            if (graphicsPrefab == null || currentGraphicsPrefab == graphicsPrefab)
                return;

            currentGraphicsPrefab = graphicsPrefab;
            if (currentGraphicInstance != null)
                Destroy(currentGraphicInstance);

            currentGraphicInstance = Instantiate(graphicsPrefab, modelRoot);
            currentGraphic = currentGraphicInstance.GetComponent<ChampionGraphic>();
        }

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
                // stat.baseStat.baseHP += level * 10f + stars * 20f;
                // stat.baseStat.baseDamage += level * 2f + stars * 5f;
                // stat.baseStat.baseArmor += level * 0.5f;

                stat.Initialize(stat.baseStat, level, stars);
            }

            // Initialize other components
            SetStatContext(stat);
            SetGraphicPrefab(config.GraphicsPrefab);

            gameObject.name = $"{config.type}_Lv{level}_Star{stars}";
            healthBarBehavior.Init(currentGraphic.transform, this, new Vector3(0, 1.5f, 0), LevelStar.Lv1, true);

        }


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
        [Button]
        public void TakeDamage(float damage)
        {
            if (stat == null) return;

            float armor = stat.GetCurrentArmor();
            float actualDamage = Mathf.Max(1f, damage - armor);

            // TODO: Implement health change logic
            stat.TakeDamage(actualDamage);

            healthBarBehavior.OnHealthChanged();

            if (CurrentHealth <= 0)
            {
                OnDeath();
            }

            OnTakenDamage?.Invoke();
        }

        /// <summary>
        /// Attack target
        /// </summary>
        public void Attack(BaseChampionBehavior target)
        {
            if (target == null) return;

            SetStatContext(stat);
            ExecuteBasicAttack(target);
        }

        /// <summary>
        /// Request champion action
        /// </summary>
        public void RequestAction(ChampionAction action)
        {
            if (currentGraphic == null) return;
            currentGraphic.PlayActionAnimation(action);
        }

        public void ExecuteBasicAttack(BaseChampionBehavior target)
        {
            if (target == null || stat == null) return;
            RequestAction(ChampionAction.BasicAttack);
        }

        public void RequestMovement(List<Vector3> pathWorldPositions)
        {
            if (movement == null) return;
            movement.MoveTo(pathWorldPositions);
        }

        /// <summary>
        /// Move champion along world positions
        /// </summary>
        public void MoveTo(List<Vector3> pathWorldPositions)
        {
            if (movement == null) return;
            movement.MoveTo(pathWorldPositions);
        }

        /// <summary>
        /// Move champion along hex coordinates
        /// </summary>
        public void MoveAlongHexPath(List<Vector3Int> hexPath)
        {
            if (movement == null) return;
            movement.MoveAlongPath(hexPath);
        }

        /// <summary>
        /// Stop current movement
        /// </summary>
        public void StopMovement()
        {
            if (movement == null) return;
            movement.StopMovement();
        }

        public void OnDeath()
        {

        }
        #endregion
    }
}
