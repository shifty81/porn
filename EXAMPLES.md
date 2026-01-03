# Example Usage Patterns

This document provides practical examples of how to use the visual novel framework.

## Example 1: Simple Dialogue Sequence

```csharp
using UnityEngine;
using VisualNovel.Core;
using VisualNovel.Data;
using System.Collections.Generic;

public class SimpleDialogueExample : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    void Start()
    {
        DialogueData dialogue = CreateSimpleDialogue();
        dialogueSystem.StartDialogue(dialogue, 0);
    }

    DialogueData CreateSimpleDialogue()
    {
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.dialogueName = "SimpleConversation";
        dialogue.nodes = new List<DialogueNode>();

        // First dialogue node
        dialogue.nodes.Add(new DialogueNode
        {
            nodeId = 0,
            dialogue = new DialogueLine
            {
                speakerName = "Alice",
                dialogueText = "Hi! Nice to meet you!",
                textSpeed = 0.05f
            },
            nextNodeId = 1,
            choices = new List<Choice>()
        });

        // Second dialogue node
        dialogue.nodes.Add(new DialogueNode
        {
            nodeId = 1,
            dialogue = new DialogueLine
            {
                speakerName = "Bob",
                dialogueText = "Nice to meet you too, Alice!",
                textSpeed = 0.05f
            },
            nextNodeId = -1, // End dialogue
            choices = new List<Choice>()
        });

        return dialogue;
    }
}
```

## Example 2: Branching Dialogue with Choices

```csharp
DialogueData CreateBranchingDialogue()
{
    DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
    dialogue.dialogueName = "BranchingConversation";
    dialogue.nodes = new List<DialogueNode>();

    // Question node
    dialogue.nodes.Add(new DialogueNode
    {
        nodeId = 0,
        dialogue = new DialogueLine
        {
            speakerName = "Alice",
            dialogueText = "Do you like coffee or tea?",
        },
        choices = new List<Choice>
        {
            new Choice
            {
                choiceText = "I love coffee!",
                nextDialogueIndex = 1,
                flagsToSet = new List<string> { "LikesCoffee" }
            },
            new Choice
            {
                choiceText = "Tea is better!",
                nextDialogueIndex = 2,
                flagsToSet = new List<string> { "LikesTea" }
            }
        }
    });

    // Coffee response
    dialogue.nodes.Add(new DialogueNode
    {
        nodeId = 1,
        dialogue = new DialogueLine
        {
            speakerName = "Alice",
            dialogueText = "Coffee is great! Let's grab some.",
        },
        nextNodeId = -1,
        choices = new List<Choice>()
    });

    // Tea response
    dialogue.nodes.Add(new DialogueNode
    {
        nodeId = 2,
        dialogue = new DialogueLine
        {
            speakerName = "Alice",
            dialogueText = "Tea is wonderful! I'll make us some.",
        },
        nextNodeId = -1,
        choices = new List<Choice>()
    });

    return dialogue;
}
```

## Example 3: Conditional Choices Based on Flags

```csharp
void CreateConditionalDialogue()
{
    // This choice only appears if player has met Alice before
    Choice conditionalChoice = new Choice
    {
        choiceText = "Hey Alice, how have you been?",
        nextDialogueIndex = 5,
        requiredFlags = new List<string> { "MetAlice" },
        flagsToSet = new List<string> { "AskedAboutAlice" }
    };

    // This choice always appears
    Choice defaultChoice = new Choice
    {
        choiceText = "Hello, I'm new here.",
        nextDialogueIndex = 10
    };
}
```

## Example 4: Managing Characters in Scene

```csharp
using UnityEngine;
using VisualNovel.Core;

public class CharacterSceneExample : MonoBehaviour
{
    public CharacterManager characterManager;

    void Start()
    {
        // Show Alice on the left with happy expression
        characterManager.ShowCharacter("Alice", "happy", new Vector2(-200, 0));
        
        // Wait 1 second, then show Bob on the right
        Invoke("ShowBob", 1f);
        
        // Wait 2 seconds, then move Alice to center
        Invoke("MoveAlice", 2f);
    }

    void ShowBob()
    {
        characterManager.ShowCharacter("Bob", "neutral", new Vector2(200, 0));
    }

    void MoveAlice()
    {
        // Move Alice to center over 1 second
        characterManager.MoveCharacter("Alice", Vector2.zero, 1f);
        
        // Change her expression to surprised
        characterManager.ChangeExpression("Alice", "surprised");
    }
}
```

## Example 5: Interactive Scene Object

```csharp
using UnityEngine;
using VisualNovel.Core;
using VisualNovel.Data;

public class InteractiveDoorExample : MonoBehaviour
{
    void Start()
    {
        // Create a clickable door
        GameObject door = new GameObject("Door");
        
        // Add sprite renderer
        SpriteRenderer sr = door.AddComponent<SpriteRenderer>();
        // sr.sprite = yourDoorSprite;
        
        // Add collider for clicking
        BoxCollider2D collider = door.AddComponent<BoxCollider2D>();
        
        // Add interactable component
        InteractableObject interactable = door.AddComponent<InteractableObject>();
        interactable.interactionName = "Mysterious Door";
        interactable.oneTimeUse = false;
        interactable.requiredFlags = new string[] { "HasKey" };
        // interactable.dialogueToTrigger = doorDialogue;
    }
}
```

## Example 6: Background Changes with Dialogue

```csharp
using UnityEngine;
using VisualNovel.Core;
using System.Collections;

public class BackgroundTransitionExample : MonoBehaviour
{
    public BackgroundManager backgroundManager;
    public DialogueSystem dialogueSystem;

    void Start()
    {
        StartCoroutine(SceneSequence());
    }

    IEnumerator SceneSequence()
    {
        // Start in bedroom
        backgroundManager.ChangeBackground("Bedroom", immediate: true);
        
        // Show dialogue
        // dialogueSystem.StartDialogue(morningDialogue, 0);
        
        // Wait for dialogue to finish
        yield return new WaitUntil(() => !dialogueSystem.IsDialogueActive());
        
        // Fade to black
        backgroundManager.FadeToBlack(1f);
        yield return new WaitForSeconds(1f);
        
        // Change to kitchen
        backgroundManager.ChangeBackground("Kitchen", immediate: true);
        
        // Fade from black
        backgroundManager.FadeFromBlack(1f);
        yield return new WaitForSeconds(1f);
        
        // Continue with next dialogue
        // dialogueSystem.StartDialogue(kitchenDialogue, 0);
    }
}
```

## Example 7: Audio Management

```csharp
using UnityEngine;
using VisualNovel.Core;

public class AudioExample : MonoBehaviour
{
    void Start()
    {
        // Play main theme on start
        AudioManager.Instance.PlayMusic("MainTheme", fadeIn: true);
    }

    public void OnEnterTenseMoment()
    {
        // Switch to tense music
        AudioManager.Instance.PlayMusic("TenseTheme", fadeIn: true);
        
        // Play tension sound effect
        AudioManager.Instance.PlaySFX("TensionRise");
    }

    public void OnRomanticMoment()
    {
        // Switch to romantic music
        AudioManager.Instance.PlayMusic("RomanticTheme", fadeIn: true);
    }

    public void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
    }
}
```

## Example 8: Save and Load Game

```csharp
using UnityEngine;
using VisualNovel.Core;

public class SaveLoadExample : MonoBehaviour
{
    public void SaveCurrentProgress()
    {
        // Save game state
        GameStateManager.Instance.SaveGame("slot1");
        Debug.Log("Game saved to slot 1");
    }

    public void LoadSavedGame()
    {
        // Load game state
        GameStateManager.Instance.LoadGame("slot1");
        Debug.Log("Game loaded from slot 1");
        
        // You might want to reload the scene or update UI here
    }

    public void StartNewGame()
    {
        // Clear all game state
        GameStateManager.Instance.NewGame();
        
        // Load first scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }

    public void AutoSave()
    {
        // Implement auto-save functionality
        GameStateManager.Instance.SaveGame("autosave");
        Debug.Log("Auto-saved");
    }
}
```

## Example 9: Flag-Based Story Progression

```csharp
using UnityEngine;
using VisualNovel.Core;

public class StoryProgressionExample : MonoBehaviour
{
    void CheckStoryProgress()
    {
        // Check which chapters are complete
        if (GameStateManager.Instance.GetFlag("Chapter1Complete"))
        {
            Debug.Log("Chapter 1 is complete!");
        }

        // Track relationship values
        int aliceRelationship = GameStateManager.Instance.GetInt("AliceRelationship", 0);
        if (aliceRelationship >= 10)
        {
            // Unlock Alice's route
            GameStateManager.Instance.SetFlag("AliceRouteUnlocked", true);
        }

        // Check multiple conditions
        bool canAccessSecretRoute = 
            GameStateManager.Instance.GetFlag("MetAlice") &&
            GameStateManager.Instance.GetFlag("MetBob") &&
            GameStateManager.Instance.GetInt("PlayerScore") >= 100;

        if (canAccessSecretRoute)
        {
            GameStateManager.Instance.SetFlag("SecretRouteAvailable", true);
        }
    }

    public void IncreaseRelationship(string characterName, int amount)
    {
        string varName = $"{characterName}Relationship";
        int current = GameStateManager.Instance.GetInt(varName, 0);
        GameStateManager.Instance.SetInt(varName, current + amount);
    }
}
```

## Example 10: Complete Scene Setup

```csharp
using UnityEngine;
using VisualNovel.Core;
using VisualNovel.Data;
using System.Collections.Generic;

public class CompleteSceneExample : MonoBehaviour
{
    [Header("Managers")]
    public DialogueSystem dialogueSystem;
    public CharacterManager characterManager;
    public BackgroundManager backgroundManager;

    [Header("Data")]
    public DialogueData introDialogue;

    void Start()
    {
        SetupScene();
        StartIntro();
    }

    void SetupScene()
    {
        // Set background
        backgroundManager.ChangeBackground("LivingRoom", immediate: true);
        
        // Play background music
        AudioManager.Instance.PlayMusic("CalmTheme", fadeIn: false);
        
        // Show characters
        characterManager.ShowCharacter("Alice", "neutral", new Vector2(-200, 0));
        characterManager.ShowCharacter("Bob", "neutral", new Vector2(200, 0));
    }

    void StartIntro()
    {
        // Check if this is the first time in this scene
        if (!GameStateManager.Instance.GetFlag("VisitedLivingRoom"))
        {
            // First visit
            GameStateManager.Instance.SetFlag("VisitedLivingRoom", true);
            dialogueSystem.StartDialogue(introDialogue, 0);
        }
        else
        {
            // Return visit - different dialogue
            dialogueSystem.StartDialogue(introDialogue, 10);
        }
    }

    public void OnDialogueComplete()
    {
        // Called when dialogue ends
        Debug.Log("Dialogue complete! Player can now explore the scene.");
        
        // Enable interactive objects
        // Show UI elements, etc.
    }
}
```

## Example 11: Mini-Game Integration

```csharp
using UnityEngine;
using VisualNovel.Core;

public class MiniGameExample : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    
    void StartMiniGame()
    {
        // Pause dialogue system
        dialogueSystem.gameObject.SetActive(false);
        
        // Start mini-game
        Debug.Log("Mini-game started!");
    }

    public void OnMiniGameComplete(bool won)
    {
        // Set flag based on result
        if (won)
        {
            GameStateManager.Instance.SetFlag("WonMiniGame", true);
            GameStateManager.Instance.SetInt("PlayerScore", 
                GameStateManager.Instance.GetInt("PlayerScore") + 10);
        }
        
        // Resume dialogue system
        dialogueSystem.gameObject.SetActive(true);
        
        // Show result dialogue
        // dialogueSystem.StartDialogue(resultDialogue, 0);
    }
}
```

## Example 12: Custom Dialogue Command Parser

```csharp
using UnityEngine;
using VisualNovel.Core;

public class CustomCommandExample : MonoBehaviour
{
    // Parse custom commands in dialogue text
    // Example: "[shake]This text shakes![/shake]"
    // Example: "[pause:2]" for 2 second pause
    
    public void ProcessDialogueCommands(string text)
    {
        if (text.Contains("[shake]"))
        {
            // Implement shake effect
            StartShakeEffect();
        }
        
        if (text.Contains("[pause:"))
        {
            int pauseStart = text.IndexOf("[pause:") + 7;
            int pauseEnd = text.IndexOf("]", pauseStart);
            string durationStr = text.Substring(pauseStart, pauseEnd - pauseStart);
            float duration = float.Parse(durationStr);
            
            // Implement pause
            StartCoroutine(PauseDialogue(duration));
        }
    }

    void StartShakeEffect()
    {
        // Shake camera or text
        Debug.Log("Shake effect triggered!");
    }

    System.Collections.IEnumerator PauseDialogue(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Pause complete!");
    }
}
```

These examples demonstrate the flexibility and power of the visual novel framework. Mix and match these patterns to create your unique game experience!
