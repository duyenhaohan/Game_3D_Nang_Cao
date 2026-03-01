using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int contactDamage = 10;
    public float contactDamageCooldown = 0.5f;

    Animator animator;
    bool isDead;
    float lastContactDamageTime;

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

    void OnTriggerEnter(Collider other)
    {
        TryContactDamage(other.gameObject);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        TryContactDamage(hit.gameObject);
    }

    void TryContactDamage(GameObject other)
    {
        if (isDead) return;
        if (!other.CompareTag("Enemy")) return;
        if (Time.time - lastContactDamageTime < contactDamageCooldown) return;

        lastContactDamageTime = Time.time;
        TakeDamage(contactDamage);
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<PlayerController>().enabled = false;
    }
}
