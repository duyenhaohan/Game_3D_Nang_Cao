using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public BossHealth bossHealth;
    
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 3.5f;
    public float rotationSpeed = 5f;
    
    [Header("Combat")]
    public float detectionRange = 15f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public int attackDamage = 30;
    
    [Header("Attack Hitbox")]
    public GameObject attackHitbox;
    
    // State
    public enum BossState { Idle, Walk, Run, Attack, Hit, Die }
    public BossState currentState = BossState.Idle;
    
    private float lastAttackTime;
    private bool isAttacking;
    private bool isDead;
    
    // Animator parameters
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int attackHash = Animator.StringToHash("Attack");
    private readonly int hitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");
    
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (bossHealth == null) bossHealth = GetComponent<BossHealth>();
        
        // Tìm player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
        // Cấu hình agent
        agent.speed = walkSpeed;
        agent.stoppingDistance = attackRange * 0.8f;
        
        // Tắt hitbox ban đầu
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
    
    void Update()
    {
        if (bossHealth.IsDead() || player == null) return;
        
        // Xử lý hành vi
        HandleBehavior();
        
        // Cập nhật animation
        UpdateAnimation();
    }
    
    void HandleBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer > detectionRange)
        {
            // Không thấy player - idle
            currentState = BossState.Idle;
            agent.ResetPath();
            return;
        }
        
        // Quay mặt về player
        FaceTarget();
        
        if (distanceToPlayer <= attackRange)
        {
            // Trong tầm đánh
            agent.ResetPath();
            
            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                Attack();
            }
            else
            {
                currentState = BossState.Idle;
            }
        }
        else
        {
            // Đuổi theo player
            agent.SetDestination(player.position);
            
            if (distanceToPlayer > detectionRange * 0.7f)
            {
                agent.speed = runSpeed;
                currentState = BossState.Run;
            }
            else
            {
                agent.speed = walkSpeed;
                currentState = BossState.Walk;
            }
        }
    }
    
    void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        currentState = BossState.Attack;
        
        // Play animation attack
        animator.SetTrigger(attackHash);
        
        Debug.Log("👹 Boss tấn công!");
        
        // Reset trạng thái attack sau 1 giây
        StartCoroutine(ResetAttack());
    }
    
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
    
    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void UpdateAnimation()
    {
        // Cập nhật speed cho animation walk/run
        float speed = agent.velocity.magnitude / runSpeed;
        animator.SetFloat(speedHash, speed);
    }
    
    // Animation Events - gọi từ animation
    public void EnableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
            Debug.Log("🔪 Boss hitbox ON");
        }
    }
    
    public void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
            Debug.Log("🔪 Boss hitbox OFF");
        }
    }
    
    public void OnAttackHit()
    {
        // Gây sát thương cho player nếu trong tầm
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"💥 Boss đánh player mất {attackDamage} máu!");
            }
        }
    }
    
    public void TakeHit()
    {
        if (isDead) return;
        
        animator.SetTrigger(hitHash);
        currentState = BossState.Hit;
    }
    
    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        currentState = BossState.Die;
        animator.SetTrigger(dieHash);
        
        agent.enabled = false;
        
        Debug.Log("👑 Boss đã chết!");
        Destroy(gameObject, 3f);
    }
}