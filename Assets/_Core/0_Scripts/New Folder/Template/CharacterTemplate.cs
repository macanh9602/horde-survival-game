using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    [System.Serializable]
    public class CharacterTemplate
    {
        [SerializeField] private string id = "hero_default";
        public string Id => id;

        [SerializeField] private string displayName = "Default Hero";
        public string DisplayName => displayName;

        [SerializeField] private int requiredLevel;
        public int RequiredLevel => requiredLevel;

        [SerializeField] private int unlockPieceRequired = 10;
        public int UnlockPieceRequired => unlockPieceRequired;

        [SerializeField] private CharacterStageDataTemplate[] stages;
        [SerializeField] private CharacterUpgradeTemplate[] upgrades;

        public CharacterStageDataTemplate[] Stages => stages;
        public CharacterUpgradeTemplate[] Upgrades => upgrades;

        private CharacterSaveTemplate save;
        public CharacterSaveTemplate Save => save;

        public void Initialise(CharacterSaveTemplate loadedSave)
        {
            save = loadedSave ?? new CharacterSaveTemplate();
        }

        public bool IsUnlocked(int currentPiece, bool unlockAll)
        {
            if (save.IsUnlocked)
                return true;

            if (unlockAll || currentPiece >= unlockPieceRequired)
            {
                save.IsUnlocked = true;
                return true;
            }

            return false;
        }

        public CharacterUpgradeTemplate GetCurrentUpgrade()
        {
            int index = Mathf.Clamp(save.UpgradeLevel - 1, 0, upgrades.Length - 1);
            return upgrades[index];
        }

        public CharacterUpgradeTemplate GetNextUpgrade()
        {
            return save.UpgradeLevel < upgrades.Length ? upgrades[save.UpgradeLevel] : null;
        }

        public CharacterStageDataTemplate GetCurrentStage()
        {
            int startIndex = Mathf.Min(save.UpgradeLevel - 1, upgrades.Length - 1);
            for (int i = startIndex; i >= 0; i--)
            {
                if (!upgrades[i].ChangeStage)
                    continue;

                int stageIndex = Mathf.Clamp(upgrades[i].StageIndex, 0, stages.Length - 1);
                return stages[stageIndex];
            }

            return stages.Length > 0 ? stages[0] : null;
        }

        public bool TryUpgrade(int currentCoin, int currentPiece)
        {
            var nextUpgrade = GetNextUpgrade();
            if (nextUpgrade == null)
                return false;

            if (!nextUpgrade.CanUpgrade(currentCoin, currentPiece))
                return false;

            save.UpgradeLevel += 1;
            return true;
        }
    }
}
