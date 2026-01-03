using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel.Data
{
    /// <summary>
    /// Represents a single line of dialogue in the visual novel
    /// </summary>
    [Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public string dialogueText;
        public string characterSprite; // Reference to character sprite
        public string backgroundSprite; // Reference to background
        public AudioClip voiceClip; // Optional voice acting
        public float textSpeed = 0.05f; // Speed of text display
    }

    /// <summary>
    /// Represents a choice the player can make
    /// </summary>
    [Serializable]
    public class Choice
    {
        public string choiceText;
        public int nextDialogueIndex; // Which dialogue node to jump to
        public List<string> requiredFlags; // Flags needed to show this choice
        public List<string> flagsToSet; // Flags to set when chosen
    }

    /// <summary>
    /// Represents a node in the dialogue tree
    /// </summary>
    [Serializable]
    public class DialogueNode
    {
        public int nodeId;
        public DialogueLine dialogue;
        public List<Choice> choices;
        public int nextNodeId = -1; // -1 means end of dialogue
        public string sceneTransition; // Scene to load after this node
    }

    /// <summary>
    /// Container for a complete dialogue sequence
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Visual Novel/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        public string dialogueName;
        public List<DialogueNode> nodes;

        public DialogueNode GetNode(int nodeId)
        {
            return nodes.Find(n => n.nodeId == nodeId);
        }
    }

    /// <summary>
    /// Represents an interactive element in a scene
    /// </summary>
    [Serializable]
    public class InteractiveElement
    {
        public string elementName;
        public string description;
        public Vector2 position;
        public DialogueData associatedDialogue;
        public bool isOneTimeUse;
        public List<string> requiredFlags;
    }
}
