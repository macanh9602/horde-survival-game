using UnityEngine;
using VTLTools;
using Watermelon;
namespace DucDevGame
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {
            SaveController.Initialise(useAutoSave: false, clearSave: true);
            ExperienceController.Instance.Initialise();
            Debug.Log(Application.persistentDataPath);
        }

        private void OnApplicationQuit()
        {
            SaveController.Save(forceSave: true);
        }

        void OnDisable()
        {
            Debug.Log($"<color=green>[DA]</color> Save by disable");
            SaveController.Save(forceSave: true);
        }
    }
}