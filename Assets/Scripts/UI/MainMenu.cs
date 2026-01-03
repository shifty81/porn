using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VisualNovel.UI
{
    /// <summary>
    /// Controls the main menu UI
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [Header("Menu Panels")]
        public GameObject mainMenuPanel;
        public GameObject loadMenuPanel;
        public GameObject settingsPanel;

        [Header("Buttons")]
        public Button newGameButton;
        public Button continueButton;
        public Button loadGameButton;
        public Button settingsButton;
        public Button exitButton;

        private void Start()
        {
            // Set up button listeners
            newGameButton.onClick.AddListener(OnNewGame);
            continueButton.onClick.AddListener(OnContinue);
            loadGameButton.onClick.AddListener(OnLoadMenu);
            settingsButton.onClick.AddListener(OnSettings);
            exitButton.onClick.AddListener(OnExit);

            // Check if there's a saved game
            bool hasSaveData = PlayerPrefs.HasKey("SaveSlot_default");
            continueButton.interactable = hasSaveData;

            ShowMainMenu();
        }

        private void OnNewGame()
        {
            Debug.Log("Starting new game...");
            VisualNovel.Core.GameStateManager.Instance.NewGame();
            
            // Load first scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        private void OnContinue()
        {
            Debug.Log("Continuing game...");
            VisualNovel.Core.GameStateManager.Instance.LoadGame("default");
            
            // Load saved scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        private void OnLoadMenu()
        {
            mainMenuPanel.SetActive(false);
            loadMenuPanel.SetActive(true);
        }

        private void OnSettings()
        {
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        private void OnExit()
        {
            Debug.Log("Exiting game...");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            loadMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
        }
    }
}
