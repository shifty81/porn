using UnityEngine;
using UnityEngine.EventSystems;
using VisualNovel.Data;

namespace VisualNovel.Core
{
    /// <summary>
    /// Handles interactive elements in scenes that can be clicked
    /// </summary>
    public class SceneInteractionManager : MonoBehaviour
    {
        [Header("References")]
        public DialogueSystem dialogueSystem;
        public Camera mainCamera;

        [Header("Interactive Elements")]
        public InteractiveElement[] sceneElements;

        [Header("Hover Settings")]
        public Color hoverColor = Color.yellow;
        public float hoverScale = 1.1f;

        private GameObject currentHoverObject;
        private Color originalColor;
        private Vector3 originalScale;

        private void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (dialogueSystem == null)
            {
                dialogueSystem = FindObjectOfType<DialogueSystem>();
            }
        }

        private void Update()
        {
            HandleMouseInteraction();
        }

        /// <summary>
        /// Handles mouse clicks and hovers on interactive elements
        /// </summary>
        private void HandleMouseInteraction()
        {
            // Don't process interactions if dialogue is active
            if (dialogueSystem != null && dialogueSystem.IsDialogueActive())
            {
                return;
            }

            // Check if mouse is over UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                ResetHover();
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
                
                if (interactable != null && interactable.CanInteract())
                {
                    // Hover effect
                    if (currentHoverObject != hit.collider.gameObject)
                    {
                        ResetHover();
                        ApplyHover(hit.collider.gameObject);
                    }

                    // Click to interact
                    if (Input.GetMouseButtonDown(0))
                    {
                        interactable.Interact();
                    }

                    // Show cursor change
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
                else
                {
                    ResetHover();
                }
            }
            else
            {
                ResetHover();
            }
        }

        /// <summary>
        /// Applies hover effect to object
        /// </summary>
        private void ApplyHover(GameObject obj)
        {
            currentHoverObject = obj;
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            
            if (sr != null)
            {
                originalColor = sr.color;
                sr.color = hoverColor;
            }

            originalScale = obj.transform.localScale;
            obj.transform.localScale = originalScale * hoverScale;
        }

        /// <summary>
        /// Resets hover effect
        /// </summary>
        private void ResetHover()
        {
            if (currentHoverObject != null)
            {
                SpriteRenderer sr = currentHoverObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = originalColor;
                }
                currentHoverObject.transform.localScale = originalScale;
                currentHoverObject = null;
            }
        }

        /// <summary>
        /// Registers an interactive element programmatically
        /// </summary>
        public void RegisterInteractiveElement(InteractiveElement element)
        {
            // Add to scene elements array
            InteractiveElement[] newArray = new InteractiveElement[sceneElements.Length + 1];
            sceneElements.CopyTo(newArray, 0);
            newArray[sceneElements.Length] = element;
            sceneElements = newArray;
        }
    }

    /// <summary>
    /// Component that makes a GameObject interactable
    /// </summary>
    public class InteractableObject : MonoBehaviour
    {
        [Header("Interaction Settings")]
        public string interactionName;
        public DialogueData dialogueToTrigger;
        public bool oneTimeUse = false;
        public string[] requiredFlags;

        [Header("Optional")]
        public Sprite highlightSprite;
        public AudioClip interactionSound;

        private bool hasBeenUsed = false;

        /// <summary>
        /// Checks if this object can be interacted with
        /// </summary>
        public bool CanInteract()
        {
            if (oneTimeUse && hasBeenUsed)
                return false;

            // Check required flags
            if (requiredFlags != null && requiredFlags.Length > 0)
            {
                foreach (string flag in requiredFlags)
                {
                    if (!GameStateManager.Instance.GetFlag(flag))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Performs the interaction
        /// </summary>
        public void Interact()
        {
            if (!CanInteract())
                return;

            Debug.Log($"Interacting with: {interactionName}");

            // Play sound if available
            if (interactionSound != null)
            {
                AudioSource.PlayClipAtPoint(interactionSound, transform.position);
            }

            // Trigger dialogue
            if (dialogueToTrigger != null)
            {
                DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
                if (dialogueSystem != null)
                {
                    dialogueSystem.StartDialogue(dialogueToTrigger);
                }
            }

            // Mark as used
            if (oneTimeUse)
            {
                hasBeenUsed = true;
                // Optionally disable visual feedback
                GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }

        /// <summary>
        /// Resets the interaction state (useful for testing)
        /// </summary>
        public void ResetInteraction()
        {
            hasBeenUsed = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
