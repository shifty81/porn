# Visual Novel Game

A Unity-based visual novel game framework with interactive scene elements and branching dialogue system.

## Features

- **Dialogue System**: Full-featured dialogue system with text animation and character-by-character display
- **Choice System**: Branching narratives with player choices that affect the story
- **Character Management**: Dynamic character sprites with multiple expressions and positions
- **Scene Interactions**: Clickable interactive elements in scenes
- **Save/Load System**: Complete game state management with save slots
- **Audio System**: Background music, sound effects, and voice acting support
- **UI System**: Main menu, in-game menu, and settings
- **Flag System**: Conditional content based on player progress
- **Background Manager**: Scene backgrounds with smooth transitions

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/               # Core game systems
│   │   ├── DialogueSystem.cs
│   │   ├── GameStateManager.cs
│   │   ├── CharacterManager.cs
│   │   ├── SceneInteractionManager.cs
│   │   ├── BackgroundManager.cs
│   │   └── AudioManager.cs
│   ├── UI/                 # UI controllers
│   │   ├── MainMenu.cs
│   │   └── GameUI.cs
│   ├── Data/               # Data structures
│   │   └── DialogueData.cs
│   └── Examples/           # Example implementations
│       └── ExampleSceneController.cs
├── Prefabs/                # Reusable game objects
├── Scenes/                 # Game scenes
├── Sprites/                # Character sprites and backgrounds
└── Audio/                  # Music and sound effects
```

## Getting Started

### Prerequisites

- Unity 2020.3 LTS or newer
- TextMeshPro package (install via Package Manager)

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/shifty81/porn.git
   cd porn
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" and select the project folder
   - Open the project

3. **Install TextMeshPro**
   - Unity will prompt you to import TMP Essentials
   - Click "Import TMP Essentials"

4. **Create Your First Scene**
   - Create a new scene in Assets/Scenes
   - Add an empty GameObject and attach `DialogueSystem` component
   - Add a Canvas with UI elements (see UI Setup below)
   - Add `GameStateManager` to manage game state
   - Add `ExampleSceneController` to test the system

## Creating Content

### Creating Dialogue

1. **Create a Dialogue Asset**
   - Right-click in Project window
   - Create > Visual Novel > Dialogue Data
   - Name your dialogue (e.g., "IntroDialogue")

2. **Add Dialogue Nodes**
   - Open the dialogue asset in Inspector
   - Add nodes with dialogue lines
   - Set speaker names and text
   - Configure choices for branching paths

3. **Example Dialogue Structure**
   ```
   Node 0:
     Speaker: "Alice"
     Text: "Hello! How are you today?"
     Next Node: 1
   
   Node 1:
     Speaker: "Player"
     Text: "[Choice Point]"
     Choices:
       - "I'm doing great!" -> Node 2
       - "Not so good..." -> Node 3
   ```

### Adding Characters

1. **Configure Character Manager**
   - Add `CharacterManager` component to scene
   - Create character entries with:
     - Character name
     - Default sprite
     - Emotion sprites (happy, sad, angry, etc.)
     - Default position

2. **Show Characters in Dialogue**
   - In DialogueData, set `characterSprite` field
   - CharacterManager will display the character automatically

### Creating Interactive Elements

1. **Add Interactive Object**
   - Add a GameObject with a SpriteRenderer
   - Add a 2D Collider component
   - Add `InteractableObject` component
   - Configure:
     - Interaction name
     - Dialogue to trigger
     - One-time use setting
     - Required flags

2. **Set Up Scene Interaction Manager**
   - Add `SceneInteractionManager` to scene
   - Assign DialogueSystem reference
   - Interactive objects will be automatically detected

## Using the Systems

### Dialogue System

```csharp
// Start a dialogue
DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
dialogueSystem.StartDialogue(myDialogueData, startNodeId: 0);

// Check if dialogue is active
if (dialogueSystem.IsDialogueActive())
{
    // Dialogue is running
}
```

### Game State Management

```csharp
// Set flags
GameStateManager.Instance.SetFlag("MetAlice", true);
GameStateManager.Instance.SetFlag("CompletedChapter1", true);

// Check flags
if (GameStateManager.Instance.GetFlag("MetAlice"))
{
    // Player has met Alice
}

// Variables
GameStateManager.Instance.SetInt("PlayerScore", 100);
int score = GameStateManager.Instance.GetInt("PlayerScore");

// Save/Load
GameStateManager.Instance.SaveGame("slot1");
GameStateManager.Instance.LoadGame("slot1");
```

### Character Management

```csharp
CharacterManager charManager = FindObjectOfType<CharacterManager>();

// Show character
charManager.ShowCharacter("Alice", "happy");

// Change expression
charManager.ChangeExpression("Alice", "surprised");

// Move character
charManager.MoveCharacter("Alice", new Vector2(100, 0), duration: 0.5f);

// Hide character
charManager.HideCharacter("Alice");
```

### Audio Management

```csharp
// Play background music
AudioManager.Instance.PlayMusic("MainTheme", fadeIn: true);

// Play sound effect
AudioManager.Instance.PlaySFX("ButtonClick");

// Play voice acting
AudioManager.Instance.PlayVoice(voiceClip);

// Adjust volumes
AudioManager.Instance.SetMusicVolume(0.7f);
AudioManager.Instance.SetSFXVolume(1.0f);
```

## UI Setup

### Main Menu Scene

1. Create a Canvas
2. Add MainMenu script
3. Create UI buttons:
   - New Game
   - Continue
   - Load Game
   - Settings
   - Exit

### Game Scene UI

1. Create a Canvas
2. Add dialogue panel with:
   - TextMeshProUGUI for speaker name
   - TextMeshProUGUI for dialogue text
   - Panel for choices
3. Add GameUI script
4. Configure references

## Input Controls

- **Space/Enter**: Advance dialogue, skip text animation
- **Mouse Click**: Select choices, interact with objects
- **ESC**: Open/close in-game menu

## Customization

### Text Speed
Adjust in DialogueSystem component or per-dialogue line

### Auto-advance
Enable in GameUI settings or DialogueSystem

### Transitions
Configure fade duration in BackgroundManager

## Tips for Adult Content Games

Since this is designed for adult visual novel content:

1. **Age Verification**: Implement age gate on startup
2. **Content Warnings**: Add content warning system
3. **Gallery System**: Consider adding a CG gallery for unlocked scenes
4. **Route System**: Track relationship values for multiple romance routes
5. **Scene Skip**: Add ability to skip scenes player has already seen

## Building the Game

1. File > Build Settings
2. Add scenes to build
3. Select target platform
4. Configure player settings
5. Click Build

## Troubleshooting

**TextMeshPro errors**: Install TMP Essentials via Window > TextMeshPro

**Dialogue not showing**: Ensure DialogueSystem has UI references assigned

**Characters not appearing**: Check CharacterManager has character database configured

**Saves not working**: Ensure GameStateManager persists between scenes (DontDestroyOnLoad)

## License

This project is open source and available for use in your games.

## Contributing

Contributions are welcome! Please feel free to submit pull requests.

## Credits

Created as a flexible visual novel framework for Unity games.
