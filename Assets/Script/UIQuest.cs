using UnityEngine;
using TMPro;

public class UIQuest : MonoBehaviour
{
    public static UIQuest Instance;

    [Header("Chest UI")]
    public TextMeshProUGUI chestText;

    [Header("Boss UI")]
    public TextMeshProUGUI bossText;

    public GameObject questPanel;

    void Awake()
    {
        Instance = this;
    }

    // ================= CHEST =================

    public void UpdateChestUI(int current, int total)
    {
        chestText.text = "Collect Chests: " + current + " / " + total;
    }

    // ================= BOSS =================

    public void UpdateBossUI(bool killed)
    {
        if (killed)
            bossText.text = "Kill Boss: Completed";
        else
            bossText.text = "Kill Boss: Incomplete";
    }

    // ================= HIDE =================

    public void HideAllQuestUI()
    {
        questPanel.SetActive(false);
    }
}