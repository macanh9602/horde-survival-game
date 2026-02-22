using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DucDevGame
{

    [CreateAssetMenu(fileName = "EquipmentSO", menuName = "ScriptableObjects/EquipmentSO")]
    public class EquipmentSO : ScriptableObject
    {
        #region Fields
        public ComponentName componentName;
        public string _name;
        public string description;
        [PreviewField(100)]
        public Sprite icon;
        public CombineEquipmentDict upgradePaths;
        public List<ComponentName> fromItems;
        public StatEquipmentDict statValues;
        #endregion

        public void OnValidate()
        {
            if (upgradePaths == null)
                upgradePaths = new CombineEquipmentDict();
            if ((int)componentName > 1000) return;
            //list ComponentName
            var componentNames = System.Enum.GetValues(typeof(ComponentName));
            foreach (ComponentName componentName in componentNames)
            {
                if ((int)componentName > 1000) return;
                if (!upgradePaths.ContainsKey(componentName) && componentName != ComponentName.None)
                {
                    upgradePaths.Add(componentName, ComponentName.None);
                }
            }
        }


    }
    [System.Serializable]
    public class CombineEquipmentDict : SerializableDictionary<ComponentName, ComponentName> { }
    [System.Serializable]
    public class StatEquipmentDict : SerializableDictionary<StatType, StatValue> { }
}