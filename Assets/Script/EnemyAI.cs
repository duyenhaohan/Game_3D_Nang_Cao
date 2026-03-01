using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyState state;

    public NavMeshAgent agent;
    public Animator animator;
    public EnemyHealth health;
    public EnemyAttack enemyAttack;

    public Transform player;
    public Transform altar;

    public float patrolRadius = 10f;
    public float detectRange = 8f;
    public float attackRange = 2f;
    public float fleeHealthPercent = 0.3f;

    private Vector3 patrolCenter;
    private float attackCooldown;
    private float nextAttackTime;
    
    // Animator parameters
    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");
    private readonly int attackHash = Animator.StringToHash("Attack");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int isDeadHash = Animator.StringToHash("IsDead");
    private readonly int inBattleHash = Animator.StringToHash("InBattle");
    private readonly int hitHash = Animator.StringToHash("Hit");

    void Start()
    {
        patrolCenter = transform.position;
        
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (health == null) health = GetComponent<EnemyHealth>();
        if (enemyAttack == null) enemyAttack = GetComponent<EnemyAttack>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        
        ChangeState(EnemyState.Patrol);
    }

    void Update()
    {
        if (health.IsDead()) return;
        if (player == null) return;

        UpdateMoveAnim();

        switch (state)
        {
            case EnemyState.Patrol: UpdatePatrol(); break;
            case EnemyState.Chase: UpdateChase(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            case EnemyState.Flee: UpdateFlee(); break;
            case EnemyState.Heal: UpdateHeal(); break;
            case EnemyState.Return: UpdateReturn(); break;
            case EnemyState.Hit: UpdateHit(); break;
        }
    }

    void ChangeState(EnemyState newState)
    {
        state = newState;
    }

    void UpdatePatrol()
    {
        animator.SetBool(inBattleHash, false);

        if (!agent.hasPath || agent.remainingDistance < 0.5f)
            MoveRandom();

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < detectRange)
            ChangeState(EnemyState.Chase);
    }

    void UpdateChase()
    {
        animator.SetBool(inBattleHash, true);
        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
            ChangeState(EnemyState.Attack);

        if (health.GetHPPercent() <= fleeHealthPercent && altar != null)
            ChangeState(EnemyState.Flee);
    }

    void UpdateAttack()
    {
        agent.ResetPath();
        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0)
        {
            animator.SetTrigger(attackHash);
            attackCooldown = 1.5f;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange)
            ChangeState(EnemyState.Chase);
    }

    void UpdateFlee()
    {
        if (altar == null)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
        
        agent.SetDestination(altar.position);

        if (Vector3.Distance(transform.position, altar.position) < 1f)
            ChangeState(EnemyState.Heal);
    }

    void UpdateHeal()
    {
        if (health != null)
        {
            health.HealOverTime();
            
            if (health.GetHPPercent() >= 1f)
                ChangeState(EnemyState.Return);
        }
    }

    void UpdateReturn()
    {
        agent.SetDestination(patrolCenter);

        if (Vector3.Distance(transform.position, patrolCenter) < 1f)
            ChangeState(EnemyState.Patrol);
    }

    void UpdateHit()
    {
        // Wait for hit animation to finish
        Invoke("ReturnToChase", 0.5f);
    }

    void ReturnToChase()
    {
        if (state == EnemyState.Hit)
            ChangeState(EnemyState.Chase);
    }

    void MoveRandom()
    {
        Vector3 rand = patrolCenter + Random.insideUnitSphere * patrolRadius;
        rand.y = patrolCenter.y;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(rand, out hit, patrolRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    void UpdateMoveAnim()
    {
        if (agent == null || animator == null) return;
        
        Vector3 vel = agent.velocity;
        Vector3 localDir = transform.InverseTransformDirection(vel);

        animator.SetFloat(moveXHash, localDir.x);
        animator.SetFloat(moveYHash, localDir.z);
        animator.SetBool(isMovingHash, vel.magnitude > 0.1f);
    }

    public void TakeHit()
    {
        ChangeState(EnemyState.Hit);
        animator.SetTrigger(hitHash);
    }
}