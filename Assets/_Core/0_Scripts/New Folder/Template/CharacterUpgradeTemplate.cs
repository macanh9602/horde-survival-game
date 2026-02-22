using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    [System.Serializable]
    public class CharacterUpgradeTemplate
    {
        [SerializeField] private int level = 1;
        public int Level => level;

        [SerializeField] private int coinCost = 100;
        public int CoinCost => coinCost;

        [SerializeField] private int pieceCost = 5;
        public int PieceCost => pieceCost;

        [SerializeField] private bool changeStage;
        public bool ChangeStage => changeStage;

        [SerializeField] private int stageIndex;
        public int StageIndex => stageIndex;

        [SerializeField] private CharacterStatsTemplate stats = new CharacterStatsTemplate();
        public CharacterStatsTemplate Stats => stats;

        public bool CanUpgrade(int currentCoin, int currentPiece)
        {
            return currentCoin >= coinCost && currentPiece >= pieceCost;
        }
    }
}
