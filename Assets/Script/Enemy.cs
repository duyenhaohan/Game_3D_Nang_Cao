using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform[] patrolPoints;
    
    public float detectRange = 10f;   // Khoảng phát hiện
    public float loseRange = 15f;     // Khoảng bỏ dấu

    private int currentPoint = 0;
    private bool chasing = false;

    void Start()
    {
        GoToNextPoint();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Nếu player vào vùng → đuổi theo
        if (dist <= detectRange)
        {
            chasing = true;
        }
        // Nếu player ra xa vùng → quay về tuần tra
        else if (dist > loseRange)
        {
            chasing = false;
        }

        if (chasing)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPoint].position);

        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}
