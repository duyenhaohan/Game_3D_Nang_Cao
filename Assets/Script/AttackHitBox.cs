using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;
    public string targetTag = "Enemy"; // "Enemy" hoặc "Player" hoặc "Boss"
    
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
        Debug.Log($"✅ Hitbox ENABLED - Target: {targetTag}");
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
        Debug.Log($"❌ Hitbox DISABLED");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == owner.transform)
            return;

        if (hasHit) return;

        if (!other.CompareTag(targetTag))
            return;

        Debug.Log($"⚔️ Hitbox trúng: {other.gameObject.name} với tag {targetTag}");

        // XỬ LÝ THEO TAG
        if (targetTag == "Enemy")
        {
            // Player đánh Enemy
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.IsDead())
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"💥 Gây {damage} sát thương cho Enemy");
                hasHit = true;
            }
        }
        else if (targetTag == "Player")
        {
            // Enemy đánh Player
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