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

        public RarityType rarity;

        public LockStatus lockStatus;

        [ShowIf("lockStatus", LockStatus.Locked)]
        public List<UnlockRequirement> unlockRequirements;
        [PreviewField(100), HideLabel]

        public GameObject GraphicsPrefab;

        public ChampionStatConfig stat;

        #endregion

        #region Properties

        #endregion

        #region Lifecycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion
    }

}
