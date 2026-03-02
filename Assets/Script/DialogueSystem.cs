using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private string[] lines;
    private int index;
    private bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        index = 0;
        isActive = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = lines[index];
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogueText.text = lines[index];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isActive = false;
        dialoguePanel.SetActive(false);
    }
}