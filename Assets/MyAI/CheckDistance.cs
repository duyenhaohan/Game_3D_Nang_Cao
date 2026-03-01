using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI")]
[TaskDescription("Kiểm tra khoảng cách tới mục tiêu")]
public class CheckDistance : Conditional
{
    public SharedGameObject target; 
    public float range = 2.0f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null) return TaskStatus.Failure;

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);
        return distance <= range ? TaskStatus.Success : TaskStatus.Failure;
    }
}