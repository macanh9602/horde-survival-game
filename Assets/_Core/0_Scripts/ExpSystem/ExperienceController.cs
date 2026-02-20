using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using Sirenix.OdinInspector;
using UnityEngine;
using VTLTools;
using Watermelon;
namespace DucDevGame
{
    public class ExperienceController : Singleton<ExperienceController>
    {
        private static readonly int SAVE_HASH = "Experience".GetHashCode();
        [SerializeField] private ExperienceDatabase expDatabase;
        [SerializeField] private ExpShopView expShopView;
        [SerializeField] private int[] expRequire;
        [ShowInInspector]
        private int currentExp;
        [ShowInInspector]
        private int currentLevel;
        public int baseExpAdd = 2;
        PlayerMatchSave save;
        public void Initialise()
        {
            save = SaveController.GetSaveObject<PlayerMatchSave>(SAVE_HASH);
            currentExp = save.currentExp;
            currentLevel = save.currentLevel;
            expRequire = new int[expDatabase.expRequireInLv.Length];
            for (int i = 0; i < expDatabase.expRequireInLv.Length; i++)
            {
                if (i == 0)
                {
                    expRequire[i] = expDatabase.expRequireInLv[i];
                    continue;
                }
                expRequire[i] = expRequire[i - 1] + expDatabase.expRequireInLv[i];
            }
        }

        public bool IsMaxLevel()
        {
            if (expDatabase == null || expDatabase.expRequireInLv.Length == 0)
                return false; // No levels defined, so not max level
            if (currentLevel >= expDatabase.expRequireInLv.Length)
                return true; // Max level reached
            return false;
        }

        public int GetLevel()
        {
            return currentLevel;
        }

        public int GetExp()
        {
            return currentExp;
        }
        [Button]
        public void AddExp(int expBonus = 0)
        {
            currentExp += baseExpAdd + expBonus;
            save.currentExp = currentExp;
            SaveController.MarkAsSaveIsRequired();
            CheckLevelUp();
        }

        public int GetCurrentExpInLevel()
        {
            if (currentLevel == 0)
                return currentExp;
            return currentExp - expRequire[currentLevel - 1];
        }

        public int GetExpRequiredForCurrentLevel()
        {
            if (currentLevel >= expRequire.Length)
                return 0; // Max level reached, no more exp required
            return expRequire[currentLevel] - (currentLevel > 0 ? expRequire[currentLevel - 1] : 0);
        }

        public float GetExpProgress()
        {
            if (IsMaxLevel())
                return 1f; // Max level reached, progress is full
            int expInCurrentLevel = GetCurrentExpInLevel();
            int expRequired = GetExpRequiredForCurrentLevel();
            //Debug.Log($"<color=green>[DA]</color> {currentLevel} : {expInCurrentLevel} / {expRequired}");
            return expRequired > 0 ? (float)expInCurrentLevel / expRequired : 0f;
        }

        public bool IsLevelUpAvailable()
        {
            if (IsMaxLevel())
                return false;

            int expRequired = expDatabase.expRequireInLv[currentLevel];
            return GetCurrentExpInLevel() >= expRequired;
        }

        public bool CheckLevelUp()
        {
            if (IsMaxLevel())
                return false;

            bool leveledUp = false;
            while (!IsMaxLevel() && IsLevelUpAvailable())
            {
                int expRequired = expDatabase.expRequireInLv[currentLevel];
                currentExp -= expRequired;
                currentLevel++;
                leveledUp = true;
            }

            if (leveledUp)
            {
                save.currentLevel = currentLevel;
                save.currentExp = currentExp;
                SaveController.MarkAsSaveIsRequired();
                SaveController.Save(forceSave: true);
            }

            return leveledUp;
        }

        //===================================================
        #region [Test]
        [Button]
        public void TestLevelUp()
        {
            currentLevel++;
            save.currentLevel = currentLevel;
            SaveController.MarkAsSaveIsRequired();
            SaveController.Save(forceSave: true);
            expShopView.UpdateUI();
        }
        #endregion
        //===================================================



    }



}