using UnityEngine;
using VisualNovel.Data;
using VisualNovel.Core;

namespace VisualNovel.Examples
{
    /// <summary>
    /// Example scene controller that demonstrates how to set up a visual novel scene
    /// </summary>
    public class ExampleSceneController : MonoBehaviour
    {
        [Header("Managers")]
        public DialogueSystem dialogueSystem;
        public CharacterManager characterManager;
        public BackgroundManager backgroundManager;
        public SceneInteractionManager interactionManager;

        [Header("Example Dialogue")]
        public DialogueData introDialogue;
        
        [Header("Settings")]
        public bool autoStartDialogue = true;

        private void Start()
        {
            // Initialize managers if not assigned
            if (dialogueSystem == null)
                dialogueSystem = FindObjectOfType<DialogueSystem>();
            
            if (characterManager == null)
                characterManager = FindObjectOfType<CharacterManager>();
            
            if (backgroundManager == null)
                backgroundManager = FindObjectOfType<BackgroundManager>();

            if (interactionManager == null)
                interactionManager = FindObjectOfType<SceneInteractionManager>();

            // Set up initial scene
            SetupScene();

            // Start intro dialogue if enabled
            if (autoStartDialogue && introDialogue != null)
            {
                StartIntroDialogue();
            }
        }

        /// <summary>
        /// Sets up the initial scene state
        /// </summary>
        private void SetupScene()
        {
            // Example: Set background
            if (backgroundManager != null)
            {
                backgroundManager.ChangeBackground("Bedroom", immediate: true);
            }

            // Example: Play background music
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic("MainTheme", fadeIn: false);
            }

            Debug.Log("Scene setup complete");
        }

        /// <summary>
        /// Starts the introduction dialogue
        /// </summary>
        private void StartIntroDialogue()
        {
            if (dialogueSystem != null && introDialogue != null)
            {
                dialogueSystem.StartDialogue(introDialogue, 0);
            }
            else
            {
                Debug.LogWarning("Cannot start intro dialogue - DialogueSystem or introDialogue is null");
            }
        }

        /// <summary>
        /// Example method to trigger a specific dialogue sequence
        /// Can be called by interactive objects
        /// </summary>
        public void TriggerDialogue(DialogueData dialogue, int startNode = 0)
        {
            if (dialogueSystem != null && dialogue != null)
            {
                dialogueSystem.StartDialogue(dialogue, startNode);
            }
        }

        /// <summary>
        /// Example method to change scene
        /// </summary>
        public void ChangeScene(string sceneName)
        {
            // Save game state before changing scene
            GameStateManager.Instance.SetString("LastScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            
            // Load new scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Example method to show a character with expression
        /// </summary>
        public void ShowCharacterWithExpression(string characterName, string expression)
        {
            if (characterManager != null)
            {
                characterManager.ShowCharacter(characterName, expression);
            }
        }
    }
}
