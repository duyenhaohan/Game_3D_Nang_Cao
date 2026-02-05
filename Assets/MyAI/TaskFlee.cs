using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI")]
public class TaskFlee : Action
{
    public SharedGameObject dangerTarget;
    public float fleeDistance = 10f;
    public float panicSpeed = 8f;
    private NavMeshAgent agent;

    public override void OnAwake() => agent = GetComponent<NavMeshAgent>();

    public override void OnStart() {
        agent.speed = panicSpeed;
        // Tính hướng ngược lại với Player
        Vector3 dirToPlayer = transform.position - dangerTarget.Value.transform.position;
        Vector3 newPos = transform.position + dirToPlayer.normalized * fleeDistance;
        agent.SetDestination(newPos);
    }

    public override TaskStatus OnUpdate() {
        if (agent.remainingDistance < 0.5f) return TaskStatus.Success; // Chạy thoát thành công
        return TaskStatus.Running;
    }
}