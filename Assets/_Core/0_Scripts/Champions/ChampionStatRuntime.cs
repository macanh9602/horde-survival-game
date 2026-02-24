using UnityEngine;
using System;

namespace DucDevGame
{
    /// <summary>
    /// Runtime stat manager with level/stars bonuses and buff/debuff delta tracking
    /// Tracks: base config -> level/star bonus -> buff/debuff modifiers -> current value
    /// </summary>
    public class ChampionStatRuntime : MonoBehaviour
    {
        #region Fields

        public ChampionStatConfig baseStat;

        // Level & Star bonuses
        private ChampionStatConfig levelStarBonus;

        // Buff/Debuff modifier (+ for buff, - for debuff)
        private ChampionStatConfig deltaModifier;

        // Current HP (tracked separately since it decreases over time)
        private float currentHP;

        // Maximum HP (updated when base/bonus/modifier changes)
        private float maxHP;

        #endregion

        #region Events
        public Action<float, float> OnHPChanged; // (currentHP, maxHP)
        public Action<string, float> OnStatChanged; // (statName, newValue)
        #endregion

        #region Lifecycle

        private void OnEnable()
        {
            // Auto-initialize if not already done
            if (levelStarBonus == null)
            {
                levelStarBonus = new ChampionStatConfig();
                deltaModifier = new ChampionStatConfig();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize runtime stats from base config with level/star bonuses
        /// </summary>
        public void Initialize(ChampionStatConfig baseStat, int level = 1, int stars = 0)
        {
            if (baseStat == null)
            {
                Debug.LogError("ChampionStatConfig is null!");
                return;
            }

            this.baseStat = baseStat;

            // Calculate max HP and set current HP to max
            maxHP = GetBaseHP();
            currentHP = maxHP;

            Debug.Log($"[ChampionStat] Initialized: HP={currentHP}/{maxHP}, DMG={GetCurrentDamage()}, ARM={GetCurrentArmor()}");
            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        /// <summary>
        /// Increase a stat by adding to delta modifier (positive = buff)
        /// </summary>
        public void IncreaseStat(string statName, float amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning($"IncreaseStat amount must be positive: {amount}");
                return;
            }

            switch (statName.ToLower())
            {
                case "hp":
                case "health":
                    HealHP(amount);
                    break;
                case "damage":
                    deltaModifier.baseDamage += amount;
                    OnStatChanged?.Invoke("Damage", GetCurrentDamage());
                    break;
                case "armor":
                    deltaModifier.baseArmor += amount;
                    OnStatChanged?.Invoke("Armor", GetCurrentArmor());
                    break;
                case "speed":
                    deltaModifier.baseSpeed += amount;
                    OnStatChanged?.Invoke("Speed", GetCurrentSpeed());
                    break;
                case "attackrange":
                case "attack_range":
                    deltaModifier.baseAttackRange += amount;
                    OnStatChanged?.Invoke("AttackRange", GetCurrentAttackRange());
                    break;
                case "attackspeed":
                case "attack_speed":
                    deltaModifier.baseAttackSpeed += amount;
                    OnStatChanged?.Invoke("AttackSpeed", GetCurrentAttackSpeed());
                    break;
                default:
                    Debug.LogWarning($"Unknown stat: {statName}");
                    break;
            }
        }

        /// <summary>
        /// Decrease a stat by subtracting from delta modifier (negative = debuff)
        /// </summary>
        public void DecreaseStat(string statName, float amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning($"DecreaseStat amount must be positive: {amount}");
                return;
            }

            switch (statName.ToLower())
            {
                case "hp":
                case "health":
                    TakeDamage(amount);
                    break;
                case "damage":
                    deltaModifier.baseDamage -= amount;
                    OnStatChanged?.Invoke("Damage", GetCurrentDamage());
                    break;
                case "armor":
                    deltaModifier.baseArmor = Mathf.Max(0, deltaModifier.baseArmor - amount);
                    OnStatChanged?.Invoke("Armor", GetCurrentArmor());
                    break;
                case "speed":
                    deltaModifier.baseSpeed = Mathf.Max(0, deltaModifier.baseSpeed - amount);
                    OnStatChanged?.Invoke("Speed", GetCurrentSpeed());
                    break;
                case "attackrange":
                case "attack_range":
                    deltaModifier.baseAttackRange = Mathf.Max(0, deltaModifier.baseAttackRange - amount);
                    OnStatChanged?.Invoke("AttackRange", GetCurrentAttackRange());
                    break;
                case "attackspeed":
                case "attack_speed":
                    deltaModifier.baseAttackSpeed = Mathf.Max(0.1f, deltaModifier.baseAttackSpeed - amount);
                    OnStatChanged?.Invoke("AttackSpeed", GetCurrentAttackSpeed());
                    break;
                default:
                    Debug.LogWarning($"Unknown stat: {statName}");
                    break;
            }
        }

        /// <summary>
        /// Direct HP healing
        /// </summary>
        public void HealHP(float amount)
        {
            if (amount <= 0)
                return;

            currentHP = Mathf.Min(currentHP + amount, maxHP);
            OnHPChanged?.Invoke(currentHP, maxHP);
            Debug.Log($"[ChampionStat] Healed: {amount} HP -> {currentHP}/{maxHP}");
        }

        /// <summary>
        /// Take damage considering armor reduction
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (damage <= 0)
                return;
            float armor = GetCurrentArmor();
            float actualDamage = Mathf.Max(1f, damage - armor);

            currentHP = Mathf.Max(0, currentHP - actualDamage);
            OnHPChanged?.Invoke(currentHP, maxHP);
            Debug.Log($"[ChampionStat] Took damage: {actualDamage} (raw={damage}, armor={armor}) -> {currentHP}/{maxHP}");
        }

        #endregion

        #region Property Getters (Current = Base + Bonus + Delta)

        public float GetCurrentHP() => currentHP;
        public float GetMaxHP() => maxHP;
        public bool IsAlive() => currentHP > 0;

        public float GetCurrentDamage() => GetBaseDamage();
        public float GetCurrentArmor() => Mathf.Max(0, GetBaseArmor());
        public float GetCurrentSpeed() => Mathf.Max(0, GetBaseSpeed());
        public float GetCurrentAttackRange() => GetBaseAttackRange();
        public float GetCurrentAttackSpeed() => Mathf.Max(0.1f, GetBaseAttackSpeed());

        #endregion

        #region Breakdown Getters (Base, Bonus, Delta)

        // Base stats from config
        private float GetBaseHP() => baseStat?.baseHP ?? 0;
        private float GetBaseDamage() => baseStat?.baseDamage ?? 0;
        private float GetBaseArmor() => baseStat?.baseArmor ?? 0;
        private float GetBaseSpeed() => baseStat?.baseSpeed ?? 0;
        private float GetBaseAttackRange() => baseStat?.baseAttackRange ?? 0;
        private float GetBaseAttackSpeed() => baseStat?.baseAttackSpeed ?? 0;

        // Bonus from level/stars

        // Delta from buff/debuff

        #endregion

        #region Debug & Info

        /// <summary>
        /// Get detailed stat breakdown for debugging
        /// </summary>
        public string GetStatBreakdown()
        {
            return $"HP: {GetCurrentHP()}/{GetMaxHP()}";
        }

        #endregion
    }
}