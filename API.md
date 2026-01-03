# API Documentation

## Core Systems API Reference

### DialogueSystem

The main system for displaying dialogue and managing conversation flow.

#### Properties

```csharp
public TextMeshProUGUI speakerNameText;    // UI text for speaker name
public TextMeshProUGUI dialogueText;       // UI text for dialogue
public GameObject dialoguePanel;            // Main dialogue panel
public GameObject choicePanel;              // Panel for choice buttons
public Button choiceButtonPrefab;           // Template for choice buttons
public float defaultTextSpeed = 0.05f;      // Speed of text animation
public bool autoAdvance = false;            // Auto-advance dialogue
public float autoAdvanceDelay = 2f;         // Delay before auto-advance
```

#### Methods

```csharp
// Starts a dialogue sequence
void StartDialogue(DialogueData dialogue, int startNodeId = 0)

// Checks if dialogue is currently active
bool IsDialogueActive()
```

#### Usage Example

```csharp
DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
DialogueData myDialogue = // ... load or reference dialogue asset
dialogueSystem.StartDialogue(myDialogue, 0);
```

---

### GameStateManager

Singleton class for managing game state, flags, and save data.

#### Accessing Instance

```csharp
GameStateManager.Instance
```

#### Flag Management Methods

```csharp
// Set a boolean flag
void SetFlag(string flagName, bool value = true)

// Get a boolean flag
bool GetFlag(string flagName)

// Check if all required flags are set
bool HasAllFlags(List<string> requiredFlags)
```

#### Variable Management Methods

```csharp
// Integer variables
void SetInt(string varName, int value)
int GetInt(string varName, int defaultValue = 0)

// String variables
void SetString(string varName, string value)
string GetString(string varName, string defaultValue = "")
```

#### Save/Load Methods

```csharp
// Save current game state to slot
void SaveGame(string saveSlot = "default")

// Load game state from slot
void LoadGame(string saveSlot = "default")

// Clear all game state (new game)
void NewGame()
```

#### Usage Example

```csharp
// Set flags
GameStateManager.Instance.SetFlag("Chapter1Complete", true);
GameStateManager.Instance.SetInt("PlayerScore", 100);

// Check flags
if (GameStateManager.Instance.GetFlag("Chapter1Complete"))
{
    // Unlock Chapter 2
}

// Save game
GameStateManager.Instance.SaveGame("autosave");
```

---

### CharacterManager

Manages character sprites, positions, and expressions.

#### Character Data Structure

```csharp
[Serializable]
public class Character
{
    public string characterName;
    public Sprite defaultSprite;
    public List<Sprite> emotionSprites;
    public Vector2 defaultPosition;
}
```

#### Methods

```csharp
// Show character on screen
void ShowCharacter(string characterName, string spriteName = "", Vector2? position = null)

// Hide character from screen
void HideCharacter(string characterName, bool immediate = false)

// Hide all characters
void HideAllCharacters()

// Change character expression
void ChangeExpression(string characterName, string expressionName)

// Move character to position
void MoveCharacter(string characterName, Vector2 targetPosition, float duration = 0.5f)
```

#### Usage Example

```csharp
CharacterManager charMgr = FindObjectOfType<CharacterManager>();

// Show character with happy expression
charMgr.ShowCharacter("Alice", "happy", new Vector2(-200, 0));

// Change to surprised expression
charMgr.ChangeExpression("Alice", "surprised");

// Move character to center
charMgr.MoveCharacter("Alice", Vector2.zero, 1f);

// Hide character
charMgr.HideCharacter("Alice");
```

---

### SceneInteractionManager

Handles interactive elements in scenes.

#### Methods

```csharp
// Register an interactive element programmatically
void RegisterInteractiveElement(InteractiveElement element)
```

#### InteractableObject Component

Attach to GameObjects to make them interactive.

```csharp
public class InteractableObject : MonoBehaviour
{
    public string interactionName;
    public DialogueData dialogueToTrigger;
    public bool oneTimeUse = false;
    public string[] requiredFlags;
    public Sprite highlightSprite;
    public AudioClip interactionSound;
    
    // Check if can interact
    bool CanInteract()
    
    // Perform interaction
    void Interact()
    
    // Reset interaction state
    void ResetInteraction()
}
```

#### Usage Example

```csharp
// Add to GameObject with SpriteRenderer and Collider2D
InteractableObject interactable = gameObject.AddComponent<InteractableObject>();
interactable.interactionName = "Mysterious Box";
interactable.dialogueToTrigger = boxDialogue;
interactable.oneTimeUse = true;
interactable.requiredFlags = new string[] { "HasKey" };
```

---

### BackgroundManager

Manages background images and transitions.

#### Background Data Structure

```csharp
[Serializable]
public class BackgroundData
{
    public string backgroundName;
    public Sprite backgroundSprite;
}
```

#### Methods

```csharp
// Change background with transition
void ChangeBackground(string backgroundName, bool immediate = false)

// Fade to black
void FadeToBlack(float duration = 1f)

// Fade from black
void FadeFromBlack(float duration = 1f)
```

#### Usage Example

```csharp
BackgroundManager bgMgr = FindObjectOfType<BackgroundManager>();

// Change to bedroom background
bgMgr.ChangeBackground("Bedroom", immediate: false);

// Fade transition
bgMgr.FadeToBlack(1f);
// ... do something ...
bgMgr.FadeFromBlack(1f);
```

---

### AudioManager

Singleton for managing music, sound effects, and voice acting.

#### Accessing Instance

```csharp
AudioManager.Instance
```

#### Audio Track Structure

```csharp
[Serializable]
public class AudioTrack
{
    public string trackName;
    public AudioClip clip;
}
```

#### Music Methods

```csharp
// Play background music
void PlayMusic(string trackName, bool fadeIn = true)

// Stop background music
void StopMusic(bool fadeOut = true)
```

#### Sound Effect Methods

```csharp
// Play sound effect by name
void PlaySFX(string sfxName)

// Play sound effect from clip
void PlaySFX(AudioClip clip)
```

#### Voice Acting Methods

```csharp
// Play voice acting clip
void PlayVoice(AudioClip voiceClip)

// Stop current voice
void StopVoice()
```

#### Volume Control Methods

```csharp
void SetMusicVolume(float volume)   // 0.0 to 1.0
void SetSFXVolume(float volume)     // 0.0 to 1.0
void SetVoiceVolume(float volume)   // 0.0 to 1.0
```

#### Usage Example

```csharp
// Play music
AudioManager.Instance.PlayMusic("MainTheme", fadeIn: true);

// Play sound effect
AudioManager.Instance.PlaySFX("DoorOpen");

// Adjust volumes
AudioManager.Instance.SetMusicVolume(0.7f);
AudioManager.Instance.SetSFXVolume(1.0f);
```

---

## Data Structures

### DialogueData

ScriptableObject for storing dialogue sequences.

```csharp
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Visual Novel/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string dialogueName;
    public List<DialogueNode> nodes;
    
    public DialogueNode GetNode(int nodeId)
}
```

### DialogueNode

Represents a node in the dialogue tree.

```csharp
[Serializable]
public class DialogueNode
{
    public int nodeId;
    public DialogueLine dialogue;
    public List<Choice> choices;
    public int nextNodeId = -1;        // -1 = end
    public string sceneTransition;
}
```

### DialogueLine

Single line of dialogue.

```csharp
[Serializable]
public class DialogueLine
{
    public string speakerName;
    public string dialogueText;
    public string characterSprite;
    public string backgroundSprite;
    public AudioClip voiceClip;
    public float textSpeed = 0.05f;
}
```

### Choice

Represents a player choice.

```csharp
[Serializable]
public class Choice
{
    public string choiceText;
    public int nextDialogueIndex;
    public List<string> requiredFlags;
    public List<string> flagsToSet;
}
```

---

## UI Components

### MainMenu

Controls the main menu UI.

```csharp
public class MainMenu : MonoBehaviour
{
    // Panels
    public GameObject mainMenuPanel;
    public GameObject loadMenuPanel;
    public GameObject settingsPanel;
    
    // Methods called by buttons
    void OnNewGame()
    void OnContinue()
    void OnLoadMenu()
    void OnSettings()
    void OnExit()
}
```

### GameUI

Controls in-game UI.

```csharp
public class GameUI : MonoBehaviour
{
    // Panels
    public GameObject gameMenuPanel;
    public GameObject dialoguePanel;
    
    // Settings controls
    public Slider textSpeedSlider;
    public Toggle autoAdvanceToggle;
    public Slider volumeSlider;
    
    // Methods
    void ToggleGameMenu()
    void HideDialoguePanel()
    void ShowDialoguePanel()
}
```

---

## Event Flow

### Starting a Dialogue Sequence

1. Call `DialogueSystem.StartDialogue(dialogue, nodeId)`
2. System loads the specified node
3. Speaker name and text are displayed
4. Text animates character by character
5. If node has choices, display choice buttons
6. If no choices, wait for player input to advance
7. When advancing, load next node or end dialogue

### Player Makes a Choice

1. Player clicks choice button
2. Associated flags are set via `GameStateManager`
3. Dialogue jumps to specified node
4. Process continues from new node

### Interactive Object Clicked

1. Player clicks object with `InteractableObject` component
2. System checks if interaction is allowed (flags, one-time use)
3. If allowed, trigger associated dialogue
4. Update object state if one-time use
5. Play interaction sound if configured

### Saving Game

1. Player opens menu and clicks Save
2. `GameStateManager.SaveGame(slot)` is called
3. Current flags and variables serialized to JSON
4. Data stored in PlayerPrefs
5. Confirmation message shown

### Loading Game

1. Player selects save slot
2. `GameStateManager.LoadGame(slot)` is called
3. JSON data retrieved from PlayerPrefs
4. Flags and variables deserialized
5. Game state restored
6. Scene continues from saved state

---

## Best Practices

### Performance

1. **Lazy Loading**: Don't load all assets at once
2. **Object Pooling**: Reuse choice buttons instead of creating/destroying
3. **Texture Atlasing**: Combine sprites to reduce draw calls

### Memory Management

1. **Unload Unused Assets**: Call `Resources.UnloadUnusedAssets()` when changing scenes
2. **Reference Cleanup**: Clear references to large objects when done
3. **Audio Streaming**: Use streaming for long music tracks

### Code Organization

1. **Namespaces**: Use `VisualNovel.Core`, `VisualNovel.UI`, etc.
2. **Regions**: Group related methods with `#region`
3. **Documentation**: Comment public APIs
4. **Naming**: Use clear, descriptive names

### Dialogue Design

1. **Node IDs**: Use sequential IDs (0, 1, 2, ...)
2. **Flag Naming**: Use descriptive names like "MetAlice" not "flag1"
3. **Validation**: Test all dialogue paths
4. **Proofreading**: Check for typos and grammar

---

## Common Patterns

### Conditional Dialogue

```csharp
// In DialogueNode setup
Choice choice1 = new Choice
{
    choiceText = "Mention Alice",
    requiredFlags = new List<string> { "MetAlice" },
    nextDialogueIndex = 5
};
```

### Scene Transitions

```csharp
// After dialogue ends, change scene
DialogueNode finalNode = new DialogueNode
{
    nodeId = 10,
    dialogue = new DialogueLine { /* ... */ },
    nextNodeId = -1,
    sceneTransition = "Chapter2Scene"
};

// Check in DialogueSystem after dialogue ends
if (!string.IsNullOrEmpty(currentNode.sceneTransition))
{
    SceneManager.LoadScene(currentNode.sceneTransition);
}
```

### Character Animation Sequence

```csharp
IEnumerator ShowCharacterSequence()
{
    CharacterManager cm = FindObjectOfType<CharacterManager>();
    
    // Show first character
    cm.ShowCharacter("Alice", "neutral", new Vector2(-200, 0));
    yield return new WaitForSeconds(0.5f);
    
    // Change expression
    cm.ChangeExpression("Alice", "happy");
    yield return new WaitForSeconds(0.5f);
    
    // Show second character
    cm.ShowCharacter("Bob", "surprised", new Vector2(200, 0));
}
```

### Background Music Changes

```csharp
void ChangeSceneMood(string mood)
{
    switch (mood)
    {
        case "tense":
            AudioManager.Instance.PlayMusic("TenseTheme", fadeIn: true);
            break;
        case "romantic":
            AudioManager.Instance.PlayMusic("RomanticTheme", fadeIn: true);
            break;
        case "action":
            AudioManager.Instance.PlayMusic("ActionTheme", fadeIn: true);
            break;
    }
}
```

---

## Troubleshooting

### Dialogue Not Appearing

- Check DialogueSystem has UI references assigned
- Verify dialogue panel is active
- Check dialogue data is not null
- Verify node IDs are correct

### Characters Not Showing

- Verify CharacterManager has character in database
- Check character name spelling
- Verify sprites are assigned
- Check characterContainer is assigned

### Choices Not Working

- Verify choiceButtonPrefab is assigned
- Check button has TextMeshProUGUI component
- Verify choicePanel is assigned
- Check nextDialogueIndex is valid

### Save/Load Not Working

- GameStateManager must persist (DontDestroyOnLoad)
- Check PlayerPrefs has write permissions
- Verify save slot names match
- Check JSON serialization errors in console

---

This API documentation covers the main systems. For specific implementation details, refer to the source code comments.
