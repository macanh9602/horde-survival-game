using System;
using UnityEngine;

namespace Watermelon.SquadShooter.Template
{
    public class CharactersControllerTemplate : MonoBehaviour
    {
        [SerializeField] private CharactersDatabaseTemplate database;

        [Header("Debug")]
        [SerializeField] private bool unlockAllCharacters;

        private CharacterGlobalSaveTemplate globalSave;
        private CharacterTemplate selectedCharacter;

        public CharacterTemplate SelectedCharacter => selectedCharacter;

        public event Action<CharacterTemplate> OnCharacterSelected;
        public event Action<CharacterTemplate> OnCharacterUpgraded;

        public void Initialise(CharacterGlobalSaveTemplate loadedGlobalSave, Func<string, CharacterSaveTemplate> loadCharacterSave)
        {
            globalSave = loadedGlobalSave ?? new CharacterGlobalSaveTemplate();

            selectedCharacter = database.GetCharacter(globalSave.SelectedCharacterId);
            if (selectedCharacter == null && database.Characters.Length > 0)
                selectedCharacter = database.Characters[0];

            for (int i = 0; i < database.Characters.Length; i++)
            {
                CharacterTemplate character = database.Characters[i];
                CharacterSaveTemplate characterSave = loadCharacterSave?.Invoke(character.Id);
                character.Initialise(characterSave);
            }
        }

        public void SelectCharacter(string characterId, CharacterBehaviourTemplate behaviour)
        {
            CharacterTemplate character = database.GetCharacter(characterId);
            if (character == null)
                return;

            selectedCharacter = character;
            globalSave.SelectedCharacterId = characterId;

            CharacterUpgradeTemplate currentUpgrade = selectedCharacter.GetCurrentUpgrade();
            CharacterStageDataTemplate currentStage = selectedCharacter.GetCurrentStage();

            behaviour.SetStats(currentUpgrade.Stats);
            behaviour.SetGraphics(currentStage.GraphicsPrefab);

            OnCharacterSelected?.Invoke(selectedCharacter);
        }

        public bool TryUpgradeSelected(CharacterBehaviourTemplate behaviour, int currentCoin, int currentPiece)
        {
            if (selectedCharacter == null)
                return false;

            bool success = selectedCharacter.TryUpgrade(currentCoin, currentPiece);
            if (!success)
                return false;

            CharacterUpgradeTemplate currentUpgrade = selectedCharacter.GetCurrentUpgrade();
            CharacterStageDataTemplate currentStage = selectedCharacter.GetCurrentStage();

            behaviour.SetStats(currentUpgrade.Stats);
            behaviour.SetGraphics(currentStage.GraphicsPrefab);

            OnCharacterUpgraded?.Invoke(selectedCharacter);
            return true;
        }

        public bool IsCharacterUnlocked(CharacterTemplate character, int currentPiece)
        {
            return character.IsUnlocked(currentPiece, unlockAllCharacters);
        }
    }
}
