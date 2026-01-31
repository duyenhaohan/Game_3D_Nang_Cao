using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    Animator animator;
    bool isDead = false;

    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        // Tắt va chạm
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        // Xoá sau khi chết
        Destroy(gameObject, 3f);
    }
}
