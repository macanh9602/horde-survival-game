using System.Collections.Generic;
using UnityEngine;
namespace DucDevGame
{
    [CreateAssetMenu(fileName = "EquipmentMappingSO", menuName = "ScriptableObjects/EquipmentMappingSO")]
    public class EquipmentMappingSO : ScriptableObject
    {
        #region Fields
        private const string ITEM_RESOURCE_FOLDER_PATH = "Data/EquipmentMappingSO";

        private static ResourceAsset<EquipmentMappingSO> asset = new(ITEM_RESOURCE_FOLDER_PATH);

        [SerializeField] List<EquipmentSO> lstConfigs = new();
        #endregion

        #region Properties

        #endregion

        #region Lifecycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        public static EquipmentSO GetData(ComponentName _type)
        {
            return asset.Value.lstConfigs.Find(x => x.componentName == _type);
        }

        public static List<EquipmentSO> GetAllDataList()
        {
            return asset.Value.lstConfigs;
        }

        
        #endregion
    }
}