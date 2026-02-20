using UnityEngine;
using Watermelon;
namespace DucDevGame
{
    public class PlayerMatchSave : ISaveObject
    {
        public int currentExp;
        public int currentLevel;
        public int currentHealth;
        public int currentRound;
        public int currentStage;
        public float remainingTime;
        public void Flush()
        {

        }
    }
}