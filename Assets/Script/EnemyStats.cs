using UnityEngine;
using BehaviorDesigner.Runtime;

public class EnemyStats : MonoBehaviour
{
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public BehaviorTree behaviorTree;

    void Start() {
        behaviorTree = GetComponent<BehaviorTree>();
    }

    void Update() {
        // Cập nhật biến "MyHealth" trong Behavior Designer
        behaviorTree.SetVariableValue("MyHealth", currentHealth / maxHealth);
    }
    
    // Hàm nhận damage (để test)
    public void TakeDamage(float amount) {
        currentHealth -= amount;
    }
}