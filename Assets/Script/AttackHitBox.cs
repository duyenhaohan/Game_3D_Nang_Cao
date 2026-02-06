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
  void OnCollisionEnter(Collision collision)
{
    Debug.Log("⚔️ Sword collided with: " + collision.gameObject.name);

    if (!collision.gameObject.CompareTag(targetTag))
    {
        Debug.Log("❌ Wrong tag: " + collision.gameObject.tag);
        return;
    }

    Debug.Log("✅ Correct target tag!");

    collision.gameObject
        .GetComponentInParent<EnemyHealth>()
        ?.TakeDamage(damage);

    Debug.Log("💥 Damage applied");

    hasHit = true;
}
}