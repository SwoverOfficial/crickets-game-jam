using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public string npcName;
    public string[] dialogueLines;
    
    public void StartTalking()
    {
        if (!DialogueUIController.Instance.IsDialogueRunning())
            DialogueUIController.Instance.EnableDialogueBox(npcName, dialogueLines);
    }
}
