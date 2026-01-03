using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VisualNovel.Core
{
    /// <summary>
    /// Manages character sprites, positions, and animations
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        [System.Serializable]
        public class Character
        {
            public string characterName;
            public Sprite defaultSprite;
            public List<Sprite> emotionSprites; // Different expressions
            public Vector2 defaultPosition;
        }

        [Header("Character Database")]
        public List<Character> characters = new List<Character>();

        [Header("Display Settings")]
        public Transform characterContainer;
        public float transitionSpeed = 1f;

        private Dictionary<string, GameObject> activeCharacters = new Dictionary<string, GameObject>();

        /// <summary>
        /// Shows a character on screen
        /// </summary>
        public void ShowCharacter(string characterName, string spriteName = "", Vector2? position = null)
        {
            Character character = characters.Find(c => c.characterName == characterName);
            if (character == null)
            {
                Debug.LogError($"Character not found: {characterName}");
                return;
            }

            GameObject charObj;
            
            // Create or get existing character object
            if (activeCharacters.ContainsKey(characterName))
            {
                charObj = activeCharacters[characterName];
            }
            else
            {
                charObj = new GameObject(characterName);
                charObj.transform.SetParent(characterContainer);
                Image image = charObj.AddComponent<Image>();
                image.sprite = character.defaultSprite;
                image.SetNativeSize();
                
                activeCharacters[characterName] = charObj;
            }

            // Update sprite if specified
            if (!string.IsNullOrEmpty(spriteName))
            {
                Sprite sprite = character.emotionSprites.Find(s => s.name == spriteName);
                if (sprite != null)
                {
                    charObj.GetComponent<Image>().sprite = sprite;
                }
            }

            // Update position
            Vector2 targetPos = position ?? character.defaultPosition;
            charObj.transform.localPosition = targetPos;
            
            charObj.SetActive(true);
        }

        /// <summary>
        /// Hides a character from screen
        /// </summary>
        public void HideCharacter(string characterName, bool immediate = false)
        {
            if (activeCharacters.ContainsKey(characterName))
            {
                if (immediate)
                {
                    activeCharacters[characterName].SetActive(false);
                }
                else
                {
                    StartCoroutine(FadeOutCharacter(activeCharacters[characterName]));
                }
            }
        }

        /// <summary>
        /// Hides all characters
        /// </summary>
        public void HideAllCharacters()
        {
            foreach (var kvp in activeCharacters)
            {
                kvp.Value.SetActive(false);
            }
        }

        /// <summary>
        /// Changes character expression
        /// </summary>
        public void ChangeExpression(string characterName, string expressionName)
        {
            if (!activeCharacters.ContainsKey(characterName))
            {
                Debug.LogWarning($"Character not active: {characterName}");
                return;
            }

            Character character = characters.Find(c => c.characterName == characterName);
            if (character == null) return;

            Sprite sprite = character.emotionSprites.Find(s => s.name == expressionName);
            if (sprite != null)
            {
                activeCharacters[characterName].GetComponent<Image>().sprite = sprite;
            }
        }

        /// <summary>
        /// Moves character to a new position
        /// </summary>
        public void MoveCharacter(string characterName, Vector2 targetPosition, float duration = 0.5f)
        {
            if (activeCharacters.ContainsKey(characterName))
            {
                StartCoroutine(MoveCharacterCoroutine(activeCharacters[characterName], targetPosition, duration));
            }
        }

        private System.Collections.IEnumerator FadeOutCharacter(GameObject charObj)
        {
            CanvasGroup cg = charObj.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = charObj.AddComponent<CanvasGroup>();
            }

            float elapsed = 0f;
            while (elapsed < transitionSpeed)
            {
                elapsed += Time.deltaTime;
                cg.alpha = 1f - (elapsed / transitionSpeed);
                yield return null;
            }

            charObj.SetActive(false);
            cg.alpha = 1f;
        }

        private System.Collections.IEnumerator MoveCharacterCoroutine(GameObject charObj, Vector2 target, float duration)
        {
            Vector2 startPos = charObj.transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                charObj.transform.localPosition = Vector2.Lerp(startPos, target, t);
                yield return null;
            }

            charObj.transform.localPosition = target;
        }
    }
}
