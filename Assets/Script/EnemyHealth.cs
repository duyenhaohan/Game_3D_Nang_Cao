using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // THÊM DÒNG NÀY

public class EnemyHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Heal")]
    public float healSpeed = 20f;   // HP / giây
    
    [Header("UI")]
    public EnemyHealthBar healthBarUI; // Kéo object có script EnemyHealthBar vào đây

    Animator animator;
    EnemyAI ai;
    NavMeshAgent agent;
    Collider col;

    bool isDead;

    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        ai = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
    }
    
    void Start()
    {
        // Khởi tạo thanh máu
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHP, maxHP);
        }
    }

    // ================= DAMAGE =================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        animator.SetTrigger("Hit");
        
        // Cập nhật thanh máu
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHP, maxHP);
        }

        if (currentHP <= 0)
            Die();
    }

    // ================= HEAL =================
    public void HealOverTime()
    {
        if (isDead) return;

        currentHP += Mathf.RoundToInt(healSpeed * Time.deltaTime);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        
        // Cập nhật thanh máu
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHP, maxHP);
        }
    }

    // ================= DIE =================
    void Die()
    {
        isDead = true;

        animator.SetBool("IsDead", true);

        if (ai != null) ai.enabled = false;
        if (agent != null) agent.isStopped = true;
        if (col != null) col.enabled = false;
        
        // Ẩn thanh máu khi chết
        if (healthBarUI != null)
        {
            healthBarUI.gameObject.SetActive(false);
        }

        Destroy(gameObject, 4f);
    }

    // ================= UTIL =================
    public float GetHPPercent()
    {
        return (float)currentHP / maxHP;
    }

    public bool IsDead()
    {
        return isDead;
    }
}