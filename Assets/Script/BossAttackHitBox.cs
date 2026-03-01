using UnityEngine;

public class BossAttackHitbox : MonoBehaviour
{
    public int damage = 50; // Damage lớn hơn enemy (enemy thường 10-20)
    public float attackCooldown = 1f; // Thời gian giữa các lần gây damage
    
    private float lastAttackTime;
    
    void OnTriggerStay(Collider other) // Dùng OnTriggerStay thay vì OnTriggerEnter
    {
        if (other.CompareTag("Player"))
        {
            // Kiểm tra cooldown
            if (Time.time - lastAttackTime < attackCooldown)
                return;
                
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
                Debug.Log($"👹 Boss chạm player gây {damage} sát thương!");
                
                // Animation tượng trưng (nếu muốn)
                Animator anim = GetComponentInParent<Animator>();
                if (anim != null)
                    anim.SetTrigger("Attack");
            }
        }
    }
}