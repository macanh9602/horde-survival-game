using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    [System.Serializable]
    public class CharacterStageDataTemplate
    {
        [SerializeField] private GameObject graphicsPrefab;
        public GameObject GraphicsPrefab => graphicsPrefab;

        [SerializeField] private Vector3 healthBarOffset;
        public Vector3 HealthBarOffset => healthBarOffset;
    }
}
