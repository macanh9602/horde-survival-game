using UnityEngine;
namespace DucDevGame
{
    // responsible behavior tree
    public class ChampionBehavior : MonoBehaviour
    {
        private ChampionStatRuntime currentStat;
        private ChampionGraphic currentGraphic;
        private GameObject currentGraphicsPrefab;

        public void SetStat(ChampionStatRuntime stat)
        {
            currentStat = stat;
        }

        public void SetGraphic(GameObject graphicsPrefab)
        {
            return;
            if (graphicsPrefab == null || currentGraphicsPrefab == graphicsPrefab)
                return;


            currentGraphicsPrefab = graphicsPrefab;
            if (currentGraphicsPrefab != null)
                Destroy(currentGraphicsPrefab.gameObject);

            GameObject graphicsObject = Instantiate(graphicsPrefab, transform);
            currentGraphic = graphicsObject.GetComponent<ChampionGraphic>();
        }
    }
}