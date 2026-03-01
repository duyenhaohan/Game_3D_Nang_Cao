using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    public GameObject hintUI;
    public Animator animator;

    bool playerInRange = false;
    bool opened = false;

    void Update()
    {
        if (playerInRange && !opened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        opened = true;
        hintUI.SetActive(false);

        if (animator != null)
        {
            animator.SetBool("isOpen", true);
        }

        // Xoá rương sau khi animation chạy xong
        Destroy(gameObject, 3f); // chỉnh thời gian theo độ dài animation
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            playerInRange = true;
            hintUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            hintUI.SetActive(false);
        }
    }
}