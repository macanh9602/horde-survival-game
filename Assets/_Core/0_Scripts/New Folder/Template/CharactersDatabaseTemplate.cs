using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    [CreateAssetMenu(fileName = "Characters Database Template", menuName = "Template/Characters Database")]
    public class CharactersDatabaseTemplate : ScriptableObject
    {
        [SerializeField] private CharacterTemplate[] characters;
        public CharacterTemplate[] Characters => characters;

        public CharacterTemplate GetCharacter(string id)
        {
            if (characters == null)
                return null;

            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].Id == id)
                    return characters[i];
            }

            return null;
        }
    }
}
