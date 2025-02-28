using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    public static DialogueUIController Instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text npcName;
    [SerializeField] private TMP_Text dialogueText;

    public List<string> dialogue;
    public float wordSpeed = 0.1f;
    private int dialogueLineIndex;
    private bool dialogueRunning;

    Coroutine speakingCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        dialogueBox.SetActive(false); //Disable at start of game
    }

    private void Update()
    {
        //MOUSE CLICK MOVES TO NEXT DIALOGUE LINE
        if (dialogueBox.activeInHierarchy && Input.GetMouseButtonDown(0))
            NextLine();
    }

    public void EnableDialogueBox(string name, string[] dialogueLines)
    {
        //DISABLE ANY PREVIOUS SPEAKING COROUTINES (to avoid duplicate letters from spawning in)
        if (speakingCoroutine != null)
            StopCoroutine(speakingCoroutine);

        ResetTextBox();

        dialogueRunning = true; //So that clicking interactable or NPC doesn't start over again when mid-way thru convo

        foreach(string line in dialogueLines) //Add in dialogue lines
            dialogue.Add(line);

        npcName.text = name;
        dialogueBox.SetActive(true);
        speakingCoroutine = StartCoroutine(Speaking());
    }

    public void DisableDialogueBox()
    {
        ResetTextBox();
        dialogueRunning = false;
        dialogueBox.SetActive(false);
    }

    public void ResetTextBox()
    {
        dialogueText.text = "";
        dialogueLineIndex = 0;
        dialogue.Clear();
    }

    IEnumerator Speaking()
    {
        //BREAKS DOWN DIALOGUE LETTER BY LETTER AND ENTERS THEM IN DIALOGUE BOX FOR TYPING EFFECT
        foreach (char letter in dialogue[dialogueLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        //DISABLE ANY PREVIOUS SPEAKING COROUTINES (to avoid duplicate letters from spawning in)
        if (speakingCoroutine != null)
            StopCoroutine(speakingCoroutine);

        //DETERMINE IF THERE ARE ANYMORE DIALOGUE LINES TO RUN, ELSE DISABLE DIALOGUE BOX
        if (dialogueLineIndex < dialogue.Count - 1)
        {
            dialogueLineIndex++;
            dialogueText.text = "";
            speakingCoroutine = StartCoroutine(Speaking());
        }
        else
        {
            DisableDialogueBox();
        }
    }

    //FUNCTION USED IN INTERACTABLE & NPC DIALOGUE SCRIPT TO DETERMINE IF THERE IS DIALOGUE CURRENTLY RUNNING (to avoid starting convo over again if you click the NPC)
    public bool IsDialogueRunning()
    {
        return dialogueRunning;
    }
}
