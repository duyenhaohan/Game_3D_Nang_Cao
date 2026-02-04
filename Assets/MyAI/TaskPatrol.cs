using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI")]
[TaskDescription("Đi tuần tra ngẫu nhiên trong bán kính")]
public class TaskPatrol : Action
{
    public float patrolRadius = 10f;
    public float moveSpeed = 3.5f;
    
    private NavMeshAgent agent;
    private Vector3 startPos;

    public override void OnAwake() {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position; // Lưu vị trí gốc
    }

    public override void OnStart() {
        agent.speed = moveSpeed;
        Vector3 randomPoint = startPos + Random.insideUnitSphere * patrolRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, 1);
        agent.SetDestination(hit.position);
    }

    public override TaskStatus OnUpdate() {
        if (agent.pathPending) return TaskStatus.Running;
        if (agent.remainingDistance < 0.5f) return TaskStatus.Success; // Đã đến nơi
        return TaskStatus.Running;
    }
}