using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    [TextArea(5,10)]
    public string[] dialogueLines;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueSystem.StartDialogue(dialogueLines);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}