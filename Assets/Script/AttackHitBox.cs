using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;
    public string targetTag; // "Enemy" hoặc "Player"

    private Collider hitbox;
    private bool hasHit;

    void Awake()
    {
        hitbox = GetComponent<Collider>();
        hitbox.enabled = false;
    }

    // ===== GỌI BẰNG ANIMATION EVENT =====
    public void EnableHitbox()
    {
        hasHit = false;
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    // ❗ DÙNG COLLISION – KHÔNG DÙNG TRIGGER
// SỬA: Đổi từ OnCollisionEnter sang OnTriggerEnter
void OnTriggerEnter(Collider other)  // Thay vì OnCollisionEnter
{
    Debug.Log("⚔️ Sword collided with: " + other.gameObject.name);

    if (!other.gameObject.CompareTag(targetTag))
    {
        Debug.Log("❌ Wrong tag: " + other.gameObject.tag);
        return;
    }

    Debug.Log("✅ Correct target tag!");

    // QUAN TRỌNG: EnemyHealth nằm trên Parent (Enemy) không phải con
    EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
    if (enemyHealth != null)
    {
        enemyHealth.TakeDamage(damage);
        Debug.Log("💥 Damage applied");
        hasHit = true;
    }
}
}