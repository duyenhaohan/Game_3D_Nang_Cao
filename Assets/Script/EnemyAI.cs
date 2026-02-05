using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyState state;

    public NavMeshAgent agent;
    public Animator animator;
    public EnemyHealth health;

    public Transform player;
    public Transform altar;

    public float patrolRadius = 10f;
    public float detectRange = 8f;
    public float attackRange = 2f;

    Vector3 patrolCenter;
    float attackCooldown;

    void Start()
    {
        patrolCenter = transform.position;
        ChangeState(EnemyState.Patrol);
    }

    void Update()
    {
        UpdateMoveAnim();

        switch (state)
        {
            case EnemyState.Patrol: UpdatePatrol(); break;
            case EnemyState.Chase: UpdateChase(); break;
            case EnemyState.Attack: UpdateAttack(); break;
            case EnemyState.Flee: UpdateFlee(); break;
            case EnemyState.Heal: UpdateHeal(); break;
            case EnemyState.Return: UpdateReturn(); break;
        }
    }

    void ChangeState(EnemyState newState)
    {
        state = newState;
    }

    // ---------- STATES ----------

    void UpdatePatrol()
    {
        animator.SetBool("InBattle", false);

        if (!agent.hasPath || agent.remainingDistance < 0.5f)
            MoveRandom();

        if (Vector3.Distance(transform.position, player.position) < detectRange)
            ChangeState(EnemyState.Chase);
    }

    void UpdateChase()
    {
        animator.SetBool("InBattle", true);
        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
            ChangeState(EnemyState.Attack);

        if (health.GetHPPercent() <= 0.3f)
            ChangeState(EnemyState.Flee);
    }

    void UpdateAttack()
    {
        agent.ResetPath();
        transform.LookAt(player);

        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0)
        {
            animator.SetTrigger("Attack");
            attackCooldown = 1.5f;
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
            ChangeState(EnemyState.Chase);
    }

    void UpdateFlee()
    {
        agent.SetDestination(altar.position);

        if (Vector3.Distance(transform.position, altar.position) < 1f)
            ChangeState(EnemyState.Heal);
    }

    void UpdateHeal()
    {
        health.currentHP += Mathf.RoundToInt(20 * Time.deltaTime);
        health.currentHP = Mathf.Min(health.currentHP, health.maxHP);

        if (health.currentHP >= health.maxHP)
            ChangeState(EnemyState.Return);
    }

    void UpdateReturn()
    {
        agent.SetDestination(patrolCenter);

        if (Vector3.Distance(transform.position, patrolCenter) < 1f)
            ChangeState(EnemyState.Patrol);
    }

    // ---------- HELPERS ----------

    void MoveRandom()
    {
        Vector3 rand = patrolCenter + Random.insideUnitSphere * patrolRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(rand, out hit, patrolRadius, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }

    void UpdateMoveAnim()
    {
        Vector3 vel = agent.velocity;
        Vector3 localDir = transform.InverseTransformDirection(vel);

        animator.SetFloat("MoveX", localDir.x);
        animator.SetFloat("MoveY", localDir.z);
        animator.SetBool("IsMoving", vel.magnitude > 0.1f);
    }
}
