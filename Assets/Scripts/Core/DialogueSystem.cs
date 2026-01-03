using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VisualNovel.Data;

namespace VisualNovel.Core
{
    /// <summary>
    /// Manages the dialogue system including text display and progression
    /// </summary>
    public class DialogueSystem : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI dialogueText;
        public GameObject dialoguePanel;
        public GameObject choicePanel;
        public Button choiceButtonPrefab;
        
        [Header("Settings")]
        public float defaultTextSpeed = 0.05f;
        public bool autoAdvance = false;
        public float autoAdvanceDelay = 2f;

        private DialogueData currentDialogue;
        private DialogueNode currentNode;
        private Coroutine textDisplayCoroutine;
        private bool isDisplayingText = false;
        private List<Button> activeChoiceButtons = new List<Button>();

        private void Update()
        {
            // Allow player to skip text animation with Space or Enter
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                if (isDisplayingText)
                {
                    SkipTextAnimation();
                }
                else if (currentNode != null && currentNode.choices.Count == 0)
                {
                    AdvanceDialogue();
                }
            }
        }

        /// <summary>
        /// Starts a dialogue sequence
        /// </summary>
        public void StartDialogue(DialogueData dialogue, int startNodeId = 0)
        {
            currentDialogue = dialogue;
            currentNode = dialogue.GetNode(startNodeId);
            
            if (currentNode == null)
            {
                Debug.LogError($"Could not find node with id {startNodeId}");
                return;
            }

            dialoguePanel.SetActive(true);
            DisplayCurrentNode();
        }

        /// <summary>
        /// Displays the current dialogue node
        /// </summary>
        private void DisplayCurrentNode()
        {
            if (currentNode == null)
            {
                EndDialogue();
                return;
            }

            DialogueLine line = currentNode.dialogue;
            
            // Update speaker name
            if (speakerNameText != null)
            {
                speakerNameText.text = line.speakerName;
            }

            // Display text with animation
            if (textDisplayCoroutine != null)
            {
                StopCoroutine(textDisplayCoroutine);
            }
            textDisplayCoroutine = StartCoroutine(DisplayTextAnimation(line));

            // Update character sprites and background if needed
            UpdateVisuals(line);
        }

        /// <summary>
        /// Animates text display character by character
        /// </summary>
        private IEnumerator DisplayTextAnimation(DialogueLine line)
        {
            isDisplayingText = true;
            dialogueText.text = "";
            
            float speed = line.textSpeed > 0 ? line.textSpeed : defaultTextSpeed;
            
            foreach (char c in line.dialogueText)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(speed);
            }
            
            isDisplayingText = false;

            // Show choices or auto-advance
            if (currentNode.choices.Count > 0)
            {
                ShowChoices();
            }
            else if (autoAdvance)
            {
                yield return new WaitForSeconds(autoAdvanceDelay);
                AdvanceDialogue();
            }
        }

        /// <summary>
        /// Skips the text animation and shows full text immediately
        /// </summary>
        private void SkipTextAnimation()
        {
            if (textDisplayCoroutine != null)
            {
                StopCoroutine(textDisplayCoroutine);
            }
            dialogueText.text = currentNode.dialogue.dialogueText;
            isDisplayingText = false;

            if (currentNode.choices.Count > 0)
            {
                ShowChoices();
            }
        }

        /// <summary>
        /// Shows available choices to the player
        /// </summary>
        private void ShowChoices()
        {
            // Clear existing choices
            foreach (Button btn in activeChoiceButtons)
            {
                Destroy(btn.gameObject);
            }
            activeChoiceButtons.Clear();

            choicePanel.SetActive(true);

            // Create choice buttons
            foreach (Choice choice in currentNode.choices)
            {
                // Check if player has required flags
                if (!GameStateManager.Instance.HasAllFlags(choice.requiredFlags))
                    continue;

                Button choiceButton = Instantiate(choiceButtonPrefab, choicePanel.transform);
                TextMeshProUGUI buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = choice.choiceText;
                
                // Capture choice in closure
                Choice currentChoice = choice;
                choiceButton.onClick.AddListener(() => OnChoiceSelected(currentChoice));
                
                activeChoiceButtons.Add(choiceButton);
            }
        }

        /// <summary>
        /// Handles choice selection
        /// </summary>
        private void OnChoiceSelected(Choice choice)
        {
            // Set flags from choice
            if (choice.flagsToSet != null)
            {
                foreach (string flag in choice.flagsToSet)
                {
                    GameStateManager.Instance.SetFlag(flag);
                }
            }

            choicePanel.SetActive(false);

            // Jump to next node
            currentNode = currentDialogue.GetNode(choice.nextDialogueIndex);
            DisplayCurrentNode();
        }

        /// <summary>
        /// Advances to the next dialogue node
        /// </summary>
        private void AdvanceDialogue()
        {
            if (currentNode.nextNodeId >= 0)
            {
                currentNode = currentDialogue.GetNode(currentNode.nextNodeId);
                DisplayCurrentNode();
            }
            else
            {
                EndDialogue();
            }
        }

        /// <summary>
        /// Updates character sprites and backgrounds
        /// </summary>
        private void UpdateVisuals(DialogueLine line)
        {
            // This would update sprites through CharacterManager
            // Implementation depends on your sprite system
            if (!string.IsNullOrEmpty(line.characterSprite))
            {
                // CharacterManager.Instance.ShowCharacter(line.characterSprite);
            }
            
            if (!string.IsNullOrEmpty(line.backgroundSprite))
            {
                // BackgroundManager.Instance.ChangeBackground(line.backgroundSprite);
            }
        }

        /// <summary>
        /// Ends the current dialogue sequence
        /// </summary>
        private void EndDialogue()
        {
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(false);
            currentDialogue = null;
            currentNode = null;
            
            Debug.Log("Dialogue ended");
        }

        /// <summary>
        /// Checks if dialogue is currently active
        /// </summary>
        public bool IsDialogueActive()
        {
            return currentDialogue != null;
        }
    }
}
