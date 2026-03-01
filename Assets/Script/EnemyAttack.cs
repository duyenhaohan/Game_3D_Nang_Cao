using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1f; // Thời gian giữa các lần gây damage
    
    private float lastAttackTime;
    
    void OnTriggerStay(Collider other) // Dùng OnTriggerStay thay vì OnTriggerEnter
    {
        // Kiểm tra nếu là player
        if (other.CompareTag("Player"))
        {
            // Kiểm tra cooldown
            if (Time.time - lastAttackTime < attackCooldown)
                return;
                
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead())
            {
                player.TakeDamage(damage);
                lastAttackTime = Time.time;
                Debug.Log("Enemy attacked player for " + damage);
            }
        }
    }
}