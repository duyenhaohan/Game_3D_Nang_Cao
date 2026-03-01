using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI")]
public class TaskAttack : Action
{
    public SharedGameObject target;
    private Animator anim;
    private NavMeshAgent agent;

    public override void OnAwake() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnStart() {
        agent.isStopped = true; // Dừng lại để đánh
        anim.SetTrigger("Attack"); // Gọi animation
        transform.LookAt(target.Value.transform.position); // Quay mặt về player
    }

    public override TaskStatus OnUpdate() {
        // Có thể thêm logic đợi animation chạy xong
        return TaskStatus.Success; 
    }

    public override void OnEnd() {
        agent.isStopped = false; // Mở lại để đi tiếp sau khi xong task
    }
}