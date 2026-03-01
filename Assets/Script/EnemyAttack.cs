using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1f;
    public AttackHitbox attackHitbox;
    
    private float lastAttackTime;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (attackHitbox == null)
            attackHitbox = GetComponentInChildren<AttackHitbox>();
        
        if (attackHitbox != null)
        {
            attackHitbox.damage = damage;
            attackHitbox.targetTag = "Player";
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime < attackCooldown)
                return;

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead())
            {
                if (animator != null)
                    animator.SetTrigger("Attack");
                    
                lastAttackTime = Time.time;
            }
        }
    }

    // Animation Events
    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.EnableHitbox();
    }
    
    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.DisableHitbox();
    }
}