using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    [System.Serializable]
    public class CharacterStatsTemplate
    {
        [SerializeField] private int health = 100;
        public int Health => health;

        [SerializeField] private float damageMultiplier = 1.0f;
        public float DamageMultiplier => damageMultiplier;

        [SerializeField] private float bonusPower;
        public float BonusPower => bonusPower;

        public void SetData(int newHealth, float newDamageMultiplier, float newBonusPower)
        {
            health = newHealth;
            damageMultiplier = newDamageMultiplier;
            bonusPower = newBonusPower;
        }
    }
}
