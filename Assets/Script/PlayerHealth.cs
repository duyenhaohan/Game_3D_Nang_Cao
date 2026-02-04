using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    Animator animator;
    bool isDead;

    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;
        Debug.Log($"Player HP: {currentHP}");

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<PlayerController>().enabled = false;
    }
}
