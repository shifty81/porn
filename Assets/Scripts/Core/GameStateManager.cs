using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel.Core
{
    /// <summary>
    /// Manages game state, flags, and save data
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager instance;
        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("GameStateManager");
                    instance = go.AddComponent<GameStateManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private Dictionary<string, bool> flags = new Dictionary<string, bool>();
        private Dictionary<string, int> intVariables = new Dictionary<string, int>();
        private Dictionary<string, string> stringVariables = new Dictionary<string, string>();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #region Flag Management
        public void SetFlag(string flagName, bool value = true)
        {
            flags[flagName] = value;
            Debug.Log($"Flag set: {flagName} = {value}");
        }

        public bool GetFlag(string flagName)
        {
            return flags.ContainsKey(flagName) && flags[flagName];
        }

        public bool HasAllFlags(List<string> requiredFlags)
        {
            if (requiredFlags == null || requiredFlags.Count == 0)
                return true;

            foreach (string flag in requiredFlags)
            {
                if (!GetFlag(flag))
                    return false;
            }
            return true;
        }
        #endregion

        #region Variable Management
        public void SetInt(string varName, int value)
        {
            intVariables[varName] = value;
        }

        public int GetInt(string varName, int defaultValue = 0)
        {
            return intVariables.ContainsKey(varName) ? intVariables[varName] : defaultValue;
        }

        public void SetString(string varName, string value)
        {
            stringVariables[varName] = value;
        }

        public string GetString(string varName, string defaultValue = "")
        {
            return stringVariables.ContainsKey(varName) ? stringVariables[varName] : defaultValue;
        }
        #endregion

        #region Save/Load System
        public void SaveGame(string saveSlot = "default")
        {
            SaveData data = new SaveData
            {
                flags = this.flags,
                intVariables = this.intVariables,
                stringVariables = this.stringVariables
            };

            string json = JsonUtility.ToJson(data, true);
            PlayerPrefs.SetString($"SaveSlot_{saveSlot}", json);
            PlayerPrefs.Save();
            Debug.Log($"Game saved to slot: {saveSlot}");
        }

        public void LoadGame(string saveSlot = "default")
        {
            string key = $"SaveSlot_{saveSlot}";
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                
                this.flags = data.flags ?? new Dictionary<string, bool>();
                this.intVariables = data.intVariables ?? new Dictionary<string, int>();
                this.stringVariables = data.stringVariables ?? new Dictionary<string, string>();
                
                Debug.Log($"Game loaded from slot: {saveSlot}");
            }
            else
            {
                Debug.LogWarning($"No save data found for slot: {saveSlot}");
            }
        }

        public void NewGame()
        {
            flags.Clear();
            intVariables.Clear();
            stringVariables.Clear();
            Debug.Log("New game started - all data cleared");
        }
        #endregion

        [System.Serializable]
        private class SaveData
        {
            public Dictionary<string, bool> flags;
            public Dictionary<string, int> intVariables;
            public Dictionary<string, string> stringVariables;
        }
    }
}
