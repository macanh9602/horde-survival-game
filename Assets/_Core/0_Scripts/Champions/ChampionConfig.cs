using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace DucDevGame
{
    [CreateAssetMenu(fileName = "ChampionConfig", menuName = "ScriptableObjects/ChampionConfig")]
    public class ChampionConfig : ScriptableObject
    {
        #region Fields
        public ChampionName type;
        public OriginType origin;

        public ClassType classType;

        public RoleType role;

        public LevelStar rarity;

        public LockStatus lockStatus;

        [ShowIf("lockStatus", LockStatus.Locked)]
        public List<UnlockRequirement> unlockRequirements;
        [PreviewField(100), HideLabel]

        public GameObject GraphicsPrefab;

        private ChampionStatConfig[] levelStarBaseStat = new ChampionStatConfig[Enum.GetValues(typeof(LevelStar)).Length];


        #endregion

        #region Properties

        #endregion

        #region Lifecycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        public ChampionStatConfig GetBaseStat(LevelStar levelStar)
        {
            int index = (int)levelStar;
            if (index < 0 || index >= levelStarBaseStat.Length)
            {
                Debug.LogError($"Invalid LevelStar value: {levelStar}");
                return null;
            }
            return levelStarBaseStat[index];
        }
        #endregion
    }

}
