using System.Collections.Generic;
using UnityEngine;
namespace DucDevGame
{
    [CreateAssetMenu(fileName = "ChampionDatabases", menuName = "ScriptableObjects/ChampionDatabases")]
    public class ChampionDatabases : ScriptableObject
    {
        #region Fields
        private const string ITEM_RESOURCE_FOLDER_PATH = "Data/ChampionDatabases";

        private static ResourceAsset<ChampionDatabases> asset = new(ITEM_RESOURCE_FOLDER_PATH);

        [SerializeField] List<ChampionConfig> lstConfigs = new();
        #endregion

        #region Properties

        #endregion

        #region Lifecycle

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        public static ChampionConfig GetData(ChampionName _type)
        {
            return asset.Value.lstConfigs.Find(x => x.type == _type);
        }

        public static List<ChampionConfig> GetAllDataList()
        {
            return asset.Value.lstConfigs;
        }
        #endregion
    }


}