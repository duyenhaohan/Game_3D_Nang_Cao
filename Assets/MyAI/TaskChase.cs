using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI")]
public class TaskChase : Action
{
    public SharedGameObject target;
    public float runSpeed = 6f;
    private NavMeshAgent agent;

    public override void OnAwake() => agent = GetComponent<NavMeshAgent>();

    public override void OnStart() => agent.speed = runSpeed;

    public override TaskStatus OnUpdate() {
        if (target.Value == null) return TaskStatus.Failure;

        agent.SetDestination(target.Value.transform.position);
        return TaskStatus.Running; // Luôn chạy đuổi theo
    }
}