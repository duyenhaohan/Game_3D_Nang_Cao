using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;
    public string targetTag = "Enemy"; // "Enemy" hoặc "Player"
    
    private Collider hitbox;
    private bool hasHit;
    private Rigidbody rb;
    private GameObject owner;

    void Awake()
    {
        hitbox = GetComponent<Collider>();
        if (hitbox == null)
            hitbox = gameObject.AddComponent<BoxCollider>();
        
        hitbox.enabled = false;
        hitbox.isTrigger = true;
        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        
        rb.isKinematic = true;
        rb.useGravity = false;
        
        owner = transform.root.gameObject;
    }

    public void EnableHitbox()
    {
        hasHit = false;
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == owner.transform)
            return;

        if (hasHit) return;

        if (!other.CompareTag(targetTag))
            return;

        Debug.Log($"⚔️ Hitbox trúng: {other.gameObject.name} với tag {targetTag}");

        // XỬ LÝ CHO CẢ ENEMY VÀ BOSS
        // Thử lấy EnemyHealth trước
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null && !enemyHealth.IsDead())
        {
            enemyHealth.TakeDamage(damage);
            Debug.Log($"💥 Gây {damage} sát thương cho Enemy");
            hasHit = true;
            return;
        }
        
        // Nếu không phải Enemy, thử lấy BossHealth
        BossHealth bossHealth = other.GetComponentInParent<BossHealth>();
        if (bossHealth != null && !bossHealth.IsDead())
        {
            bossHealth.TakeDamage(damage);
            Debug.Log($"💥 Gây {damage} sát thương cho BOSS");
            hasHit = true;
            return;
        }
        
        // Xử lý Player (nếu target là Player)
        if (targetTag == "Player")
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"💥 Gây {damage} sát thương cho Player");
                hasHit = true;
            }
        }
    }
}
