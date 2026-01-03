# Quick Setup Guide

This guide will help you get started with the Visual Novel framework in just a few minutes.

## Prerequisites

1. Unity 2020.3 LTS or newer
2. Basic knowledge of Unity Editor
3. TextMeshPro package

## Step 1: Open the Project

1. Clone or download the repository
2. Open Unity Hub
3. Click "Add" and select the project folder
4. Open the project in Unity

## Step 2: Import TextMeshPro

When you first open the project, Unity will prompt you to import TextMeshPro Essentials:

1. Window > TextMeshPro > Import TMP Essential Resources
2. Click "Import"

## Step 3: Create Your First Scene

### Option A: Use the Sample Scene

1. Open `Assets/Scenes/SampleScene.unity`
2. This is a basic scene with a camera

### Option B: Create a New Scene

1. File > New Scene
2. Select "2D" template
3. Save as "GameScene" in Assets/Scenes

## Step 4: Set Up the Dialogue System

1. **Create Canvas**
   - GameObject > UI > Canvas
   - Set Canvas Scaler to "Scale With Screen Size"
   - Reference Resolution: 1920x1080

2. **Create Dialogue Panel**
   - Right-click Canvas > UI > Panel
   - Name it "DialoguePanel"
   - Position at bottom of screen
   - Add Image component (dark background)

3. **Add Dialogue Text**
   - Right-click DialoguePanel > UI > Text - TextMeshPro
   - Name it "DialogueText"
   - Set font size to 24
   - Set color to white
   - Set alignment to top-left

4. **Add Speaker Name Text**
   - Right-click DialoguePanel > UI > Text - TextMeshPro
   - Name it "SpeakerNameText"
   - Position above DialogueText
   - Set font size to 28, bold
   - Set color to yellow/gold

5. **Create Choice Panel**
   - Right-click Canvas > UI > Panel
   - Name it "ChoicePanel"
   - Position in center of screen
   - Set to inactive initially

6. **Create Choice Button Prefab**
   - Create > UI > Button - TextMeshPro
   - Name it "ChoiceButton"
   - Style as desired
   - Drag into Assets/Prefabs folder
   - Delete from scene

7. **Add DialogueSystem Script**
   - Create empty GameObject: "GameManager"
   - Add Component > DialogueSystem
   - Assign references:
     - Speaker Name Text
     - Dialogue Text
     - Dialogue Panel
     - Choice Panel
     - Choice Button Prefab

## Step 5: Create Your First Dialogue

1. **Create Dialogue Data Asset**
   - Right-click in Project > Create > Visual Novel > Dialogue Data
   - Name it "IntroDialogue"

2. **Configure Dialogue** (via script or Inspector)
   
   Since Unity doesn't show custom inspectors initially, you'll configure dialogues programmatically or create a custom editor. Here's a script example:

   ```csharp
   // Example: Create dialogue in code
   DialogueData intro = ScriptableObject.CreateInstance<DialogueData>();
   intro.dialogueName = "Introduction";
   
   DialogueNode node0 = new DialogueNode
   {
       nodeId = 0,
       dialogue = new DialogueLine
       {
           speakerName = "Narrator",
           dialogueText = "Welcome to the visual novel!",
           textSpeed = 0.05f
       },
       nextNodeId = 1
   };
   
   intro.nodes.Add(node0);
   ```

## Step 6: Set Up Characters (Optional)

1. **Add CharacterManager**
   - Add to GameManager object
   - Create > Empty GameObject: "CharacterContainer" under Canvas
   - Assign to CharacterManager

2. **Add Character Sprites**
   - Import character images to Assets/Sprites
   - Configure in CharacterManager inspector

## Step 7: Set Up Background

1. **Add BackgroundManager**
   - Add to GameManager object
   - Create UI > Image in Canvas
   - Name it "BackgroundImage"
   - Set to stretch to full canvas
   - Send to back (move to top of hierarchy)
   - Assign to BackgroundManager

## Step 8: Test the System

1. **Create Test Script**

   Create `Assets/Scripts/TestDialogue.cs`:

   ```csharp
   using UnityEngine;
   using VisualNovel.Core;
   using VisualNovel.Data;
   using System.Collections.Generic;

   public class TestDialogue : MonoBehaviour
   {
       public DialogueSystem dialogueSystem;

       void Start()
       {
           // Create test dialogue
           DialogueData testDialogue = CreateTestDialogue();
           
           // Start dialogue after 1 second
           Invoke("StartTest", 1f);
       }

       void StartTest()
       {
           DialogueData testDialogue = CreateTestDialogue();
           dialogueSystem.StartDialogue(testDialogue, 0);
       }

       DialogueData CreateTestDialogue()
       {
           DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
           dialogue.dialogueName = "Test";
           dialogue.nodes = new List<DialogueNode>();

           // Node 0
           DialogueNode node0 = new DialogueNode
           {
               nodeId = 0,
               dialogue = new DialogueLine
               {
                   speakerName = "System",
                   dialogueText = "Hello! This is a test dialogue. Press Space or Enter to continue.",
                   textSpeed = 0.05f
               },
               nextNodeId = 1,
               choices = new List<Choice>()
           };
           dialogue.nodes.Add(node0);

           // Node 1
           DialogueNode node1 = new DialogueNode
           {
               nodeId = 1,
               dialogue = new DialogueLine
               {
                   speakerName = "System",
                   dialogueText = "Now you can choose an option!",
                   textSpeed = 0.05f
               },
               nextNodeId = -1,
               choices = new List<Choice>
               {
                   new Choice
                   {
                       choiceText = "Option 1",
                       nextDialogueIndex = 2
                   },
                   new Choice
                   {
                       choiceText = "Option 2",
                       nextDialogueIndex = 3
                   }
               }
           };
           dialogue.nodes.Add(node1);

           // Node 2
           DialogueNode node2 = new DialogueNode
           {
               nodeId = 2,
               dialogue = new DialogueLine
               {
                   speakerName = "System",
                   dialogueText = "You chose Option 1! End of test.",
                   textSpeed = 0.05f
               },
               nextNodeId = -1,
               choices = new List<Choice>()
           };
           dialogue.nodes.Add(node2);

           // Node 3
           DialogueNode node3 = new DialogueNode
           {
               nodeId = 3,
               dialogue = new DialogueLine
               {
                   speakerName = "System",
                   dialogueText = "You chose Option 2! End of test.",
                   textSpeed = 0.05f
               },
               nextNodeId = -1,
               choices = new List<Choice>()
           };
           dialogue.nodes.Add(node3);

           return dialogue;
       }
   }
   ```

2. **Add Test Script**
   - Add TestDialogue component to GameManager
   - Assign DialogueSystem reference
   - Play the scene!

## Step 9: Add Interactive Objects (Optional)

1. **Create Clickable Object**
   - Create 2D Sprite in scene
   - Add BoxCollider2D
   - Add InteractableObject component
   - Configure interaction settings

2. **Add Scene Interaction Manager**
   - Add to GameManager
   - Assign DialogueSystem reference

## Step 10: Build Complete Scene

Now you're ready to build a complete scene:

1. Add backgrounds
2. Add character sprites
3. Create full dialogue trees
4. Add interactive objects
5. Set up background music
6. Test all paths

## Common Issues

### "TextMeshPro not found"
- Import TMP Essentials via Window > TextMeshPro

### "Dialogue doesn't show"
- Check all references are assigned in DialogueSystem
- Verify DialoguePanel is active
- Check console for errors

### "Can't click choices"
- Ensure ChoiceButtonPrefab has Button component
- Check EventSystem exists in scene
- Verify Canvas has GraphicRaycaster

### "Characters don't appear"
- Check CharacterContainer is assigned
- Verify character sprites are in database
- Check character names match exactly

## Next Steps

1. Read the full README.md for detailed documentation
2. Check API.md for code reference
3. Review DESIGN.md for game design patterns
4. Create your own dialogue content
5. Add your own art assets
6. Build your game!

## Getting Help

- Check the documentation files
- Review example scripts
- Look at console error messages
- Test incrementally

Good luck with your visual novel!
