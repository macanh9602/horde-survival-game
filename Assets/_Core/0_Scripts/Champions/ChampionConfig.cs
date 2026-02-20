using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace DucDevGame
{
    [CreateAssetMenu(fileName = "ChampionConfig", menuName = "ScriptableObjects/ChampionConfig")]
    public class ChampionConfig : ScriptableObject
    {
        #region Fields
        [VerticalGroup("0/1"), LabelWidth(100)]
        public ChampionName type;
        [VerticalGroup("0/1"), LabelWidth(100)]
        public OriginType origin;
        [VerticalGroup("0/1"), LabelWidth(100)]
        public ClassType classType;
        [VerticalGroup("0/1"), LabelWidth(100)]
        public RoleType role;
        [VerticalGroup("0/1"), LabelWidth(100)]
        public RarityType rarity;
        [VerticalGroup("0/1"), LabelWidth(100)]
        public LockStatus lockStatus;
        [VerticalGroup("0/1"), LabelWidth(100)]
        [ShowIf("lockStatus", LockStatus.Locked)]
        public List<UnlockRequirement> unlockRequirements;
        [PreviewField(100), HideLabel, HorizontalGroup("0", 100)]
        public Sprite icon;

        [VerticalGroup("0/1"), LabelWidth(100)]
        public UnitBase prefab;

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
