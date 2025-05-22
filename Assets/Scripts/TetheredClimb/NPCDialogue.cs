using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject dialogueUI;              
    public TextMeshProUGUI dialogueText;       
    public GameObject ePrompt;                

    [Header("Diálogo")]
    [TextArea]
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;
    private bool isPlayerInRange = false;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        dialogueUI.SetActive(false);
        ePrompt.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueUI.activeSelf)
            {
                ePrompt.SetActive(false);
                StartDialogue();
            }
            else if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < dialogueLines.Length)
                {
                    StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
                }
                else
                {
                    EndDialogue();
                    StartMission();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!dialogueUI.activeSelf)
                ePrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            EndDialogue();
            ePrompt.SetActive(false);
        }
    }

    void StartDialogue()
    {
        AudioManager.I.DuckBGM();                               
        AudioManager.I.PlayVoice(AudioManager.I.speakingSound, 0.8f);

        currentLineIndex = 0;
        dialogueUI.SetActive(true);
        StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
    }

    void EndDialogue()
    {
        AudioManager.I.StopVoice();  
        AudioManager.I.UnduckBGM();   

        dialogueUI.SetActive(false);
        StopAllCoroutines();
        isTyping = false;
    }





    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            if (skipTyping)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void StartMission()
    {
        GameManager.instance.StartMission();

    }

}
