using UnityEngine;
namespace DucDevGame
{


    public class ChampionStatRuntime : MonoBehaviour
    {
        public ChampionStatConfig baseStat;

        public float GetCurrentHP()
        {
            //base + upgradeHP + buffHP - debuffHP
            return baseStat.baseHP;
        }

        public float GetCurrentDamage()
        {
            return baseStat.baseDamage;
        }

        public float GetCurrentArmor()
        {
            return baseStat.baseArmor;
        }

        public float GetCurrentSpeed()
        {
            return baseStat.baseSpeed;
        }

        public float GetCurrentAttackRange()
        {
            return baseStat.baseAttackRange;
        }

        public float GetCurrentAttackSpeed()
        {
            return baseStat.baseAttackSpeed;
        }
    }
}