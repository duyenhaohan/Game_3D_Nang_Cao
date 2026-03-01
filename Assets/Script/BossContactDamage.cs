using UnityEngine;
using UnityEngine.AI;

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
    
    // Animator parameters
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int hitHash = Animator.StringToHash("getHit");
    private readonly int dieHash = Animator.StringToHash("die");
    
    private bool isDead;
    
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (bossHealth == null) bossHealth = GetComponent<BossHealth>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
        agent.speed = walkSpeed;
        agent.stoppingDistance = attackRange * 0.8f;
    }
    
    void Update()
    {
        if (bossHealth.IsDead() || player == null) return;
        
        HandleBehavior();
        UpdateAnimation();
    }
    
    void HandleBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer > detectionRange)
        {
            agent.ResetPath();
            return;
        }
        
        FaceTarget();
        
        if (distanceToPlayer <= attackRange)
        {
            // Trong tầm đánh - dừng lại
            agent.ResetPath();
            
            // Animation tấn công (tượng trưng)
            if (animator != null)
                animator.SetTrigger("Attack");
        }
        else
        {
            // Đuổi theo player
            agent.SetDestination(player.position);
            
            if (distanceToPlayer > detectionRange * 0.7f)
                agent.speed = runSpeed;
            else
                agent.speed = walkSpeed;
        }
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
        float speed = agent.velocity.magnitude / runSpeed;
        animator.SetFloat(speedHash, speed);
    }
    
    public void TakeHit()
    {
        if (isDead) return;
        animator.SetTrigger(hitHash);
    }
    
    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        animator.SetTrigger(dieHash);
        
        agent.enabled = false;
        enabled = false;
        
        Debug.Log("👑 Boss đã chết!");
        Destroy(gameObject, 3f);
    }
}