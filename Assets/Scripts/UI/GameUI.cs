using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VisualNovel.UI
{
    /// <summary>
    /// Controls the in-game UI elements
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject gameMenuPanel;
        public GameObject dialoguePanel;

        [Header("Buttons")]
        public Button menuButton;
        public Button saveButton;
        public Button loadButton;
        public Button returnToMainMenuButton;
        public Button resumeButton;

        [Header("Settings")]
        public Slider textSpeedSlider;
        public Toggle autoAdvanceToggle;
        public Slider volumeSlider;

        private bool isMenuOpen = false;

        private void Start()
        {
            // Set up button listeners
            if (menuButton != null)
                menuButton.onClick.AddListener(ToggleGameMenu);
            
            if (saveButton != null)
                saveButton.onClick.AddListener(OnSave);
            
            if (loadButton != null)
                loadButton.onClick.AddListener(OnLoad);
            
            if (returnToMainMenuButton != null)
                returnToMainMenuButton.onClick.AddListener(OnReturnToMainMenu);
            
            if (resumeButton != null)
                resumeButton.onClick.AddListener(ToggleGameMenu);

            // Set up settings controls
            if (textSpeedSlider != null)
                textSpeedSlider.onValueChanged.AddListener(OnTextSpeedChanged);
            
            if (autoAdvanceToggle != null)
                autoAdvanceToggle.onValueChanged.AddListener(OnAutoAdvanceToggled);
            
            if (volumeSlider != null)
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

            // Hide menu initially
            if (gameMenuPanel != null)
                gameMenuPanel.SetActive(false);

            LoadSettings();
        }

        private void Update()
        {
            // Toggle menu with ESC key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleGameMenu();
            }
        }

        private void ToggleGameMenu()
        {
            isMenuOpen = !isMenuOpen;
            
            if (gameMenuPanel != null)
                gameMenuPanel.SetActive(isMenuOpen);

            // Pause/unpause game
            Time.timeScale = isMenuOpen ? 0f : 1f;
        }

        private void OnSave()
        {
            VisualNovel.Core.GameStateManager.Instance.SaveGame("default");
            ShowNotification("Game Saved!");
        }

        private void OnLoad()
        {
            VisualNovel.Core.GameStateManager.Instance.LoadGame("default");
            ShowNotification("Game Loaded!");
            ToggleGameMenu();
        }

        private void OnReturnToMainMenu()
        {
            Time.timeScale = 1f; // Unpause
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        private void OnTextSpeedChanged(float value)
        {
            PlayerPrefs.SetFloat("TextSpeed", value);
            // Update dialogue system text speed
            var dialogueSystem = FindObjectOfType<VisualNovel.Core.DialogueSystem>();
            if (dialogueSystem != null)
            {
                dialogueSystem.defaultTextSpeed = 0.1f - (value * 0.09f); // 0.1 to 0.01
            }
        }

        private void OnAutoAdvanceToggled(bool value)
        {
            PlayerPrefs.SetInt("AutoAdvance", value ? 1 : 0);
            // Update dialogue system
            var dialogueSystem = FindObjectOfType<VisualNovel.Core.DialogueSystem>();
            if (dialogueSystem != null)
            {
                dialogueSystem.autoAdvance = value;
            }
        }

        private void OnVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("Volume", value);
            AudioListener.volume = value;
        }

        private void LoadSettings()
        {
            // Load saved settings
            if (textSpeedSlider != null)
            {
                float textSpeed = PlayerPrefs.GetFloat("TextSpeed", 0.5f);
                textSpeedSlider.value = textSpeed;
            }

            if (autoAdvanceToggle != null)
            {
                bool autoAdvance = PlayerPrefs.GetInt("AutoAdvance", 0) == 1;
                autoAdvanceToggle.isOn = autoAdvance;
            }

            if (volumeSlider != null)
            {
                float volume = PlayerPrefs.GetFloat("Volume", 1f);
                volumeSlider.value = volume;
                AudioListener.volume = volume;
            }
        }

        private void ShowNotification(string message)
        {
            Debug.Log($"[Notification] {message}");
            // TODO: Implement proper notification UI
        }

        public void HideDialoguePanel()
        {
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }

        public void ShowDialoguePanel()
        {
            if (dialoguePanel != null)
                dialoguePanel.SetActive(true);
        }
    }
}
