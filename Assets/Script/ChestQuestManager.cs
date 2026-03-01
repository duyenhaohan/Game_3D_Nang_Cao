using UnityEngine;

public class ChestQuestManager : MonoBehaviour
{
    public static ChestQuestManager Instance;

    [Header("Chest Quest")]
    public int totalChests = 3;
    private int collectedChests = 0;

    [Header("Boss Quest")]
    private bool bossKilled = false;
    [Header("Reward NPC")]
    public GameObject rewardNPC;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UIQuest.Instance.UpdateChestUI(collectedChests, totalChests);
        UIQuest.Instance.UpdateBossUI(false);
    }

    // ================= CHEST =================

    public void AddChest()
    {
        collectedChests++;

        UIQuest.Instance.UpdateChestUI(collectedChests, totalChests);

        CheckAllQuestComplete();
    }

    // ================= BOSS =================

    public void BossKilled()
    {
        bossKilled = true;

        UIQuest.Instance.UpdateBossUI(true);

        CheckAllQuestComplete();
    }

    // ================= CHECK =================

    void CheckAllQuestComplete()
    {
        if (collectedChests >= totalChests && bossKilled)
        {
            Debug.Log("ALL QUEST COMPLETED!");

            if (rewardNPC != null)
                rewardNPC.SetActive(true);

            UIQuest.Instance.HideAllQuestUI();
        }
    }
}