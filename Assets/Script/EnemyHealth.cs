using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Heal")]
    public float healSpeed = 20f;   // HP / giây

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

    // ================= DAMAGE =================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        animator.SetTrigger("Hit");

        if (currentHP <= 0)
            Die();
    }

    // ================= HEAL =================
    public void HealOverTime()
    {
        if (isDead) return;

        currentHP += Mathf.RoundToInt(healSpeed * Time.deltaTime);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    // ================= DIE =================
    void Die()
    {
        isDead = true;

        animator.SetBool("IsDead", true);

        if (ai != null) ai.enabled = false;
        if (agent != null) agent.isStopped = true;
        if (col != null) col.enabled = false;

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
