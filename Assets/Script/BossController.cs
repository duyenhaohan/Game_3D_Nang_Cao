using UnityEngine;
using UnityEngine.AI;

public class BossController1 : MonoBehaviour
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
    
    [Header("Detection")]
    public float detectionRange = 15f;
    public float stopRange = 3f;
    
    private float speedHash;
    private float hitHash;
    private float dieHash;
    private bool isDead;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bossHealth = GetComponent<BossHealth>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
        agent.speed = walkSpeed;
        agent.stoppingDistance = stopRange;
    }
    
    void Update()
    {
        if (bossHealth.IsDead() || player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Quay mặt về player
            FaceTarget();
            
            // Di chuyển đến player
            agent.SetDestination(player.position);
            
            // Điều chỉnh tốc độ
            if (distanceToPlayer > detectionRange * 0.7f)
                agent.speed = runSpeed;
            else
                agent.speed = walkSpeed;
        }
        else
        {
            agent.ResetPath();
        }
        
        // Cập nhật animation
        float speed = agent.velocity.magnitude / runSpeed;
        animator.SetFloat("Speed", speed);
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
    
    public void TakeHit()
    {
        if (isDead) return;
        animator.SetTrigger("Hit");
    }
    
    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        animator.SetTrigger("Die");
        
        agent.enabled = false;
        enabled = false;
        
        Destroy(gameObject, 3f);
    }
}