using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    public class CharacterBehaviourTemplate : MonoBehaviour
    {
        private CharacterStatsTemplate currentStats;
        private BaseCharacterGraphicsTemplate currentGraphics;
        private GameObject currentGraphicsPrefab;

        public CharacterStatsTemplate CurrentStats => currentStats;

        public void SetStats(CharacterStatsTemplate stats)
        {
            currentStats = stats;
        }

        public void SetGraphics(GameObject graphicsPrefab)
        {
            if (graphicsPrefab == null || currentGraphicsPrefab == graphicsPrefab)
                return;

            currentGraphicsPrefab = graphicsPrefab;

            if (currentGraphics != null)
                Destroy(currentGraphics.gameObject);

            GameObject graphicsObject = Instantiate(graphicsPrefab, transform);
            currentGraphics = graphicsObject.GetComponent<BaseCharacterGraphicsTemplate>();

            if (currentGraphics != null)
                currentGraphics.Initialise(this);
        }

        public void SetMoveVisual(float speedPercent, Vector3 direction, bool hasTarget)
        {
            currentGraphics?.OnMoving(speedPercent, direction, hasTarget);
        }
    }
}
