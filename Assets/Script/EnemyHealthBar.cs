using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Transform enemyTransform;
    public Vector3 offset = new Vector3(0, 2f, 0); // Điều chỉnh độ cao
    
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        // Tự động tìm slider
        if (healthBar == null)
            healthBar = GetComponentInChildren<Slider>();
            
        // Tự động tìm enemy (cha của canvas)
        if (enemyTransform == null)
            enemyTransform = transform.parent;
            
        Debug.Log("HealthBar started for: " + enemyTransform.name);
    }
    
    void LateUpdate() // Dùng LateUpdate để tránh giật
    {
        if (mainCamera == null) 
        {
            mainCamera = Camera.main;
            return;
        }
        
        if (enemyTransform != null)
        {
            // Luôn quay về phía camera
            transform.rotation = mainCamera.transform.rotation;
            
            // Đặt vị trí trên đầu enemy
            Vector3 worldPos = enemyTransform.position + offset;
            transform.position = worldPos;
        }
    }
    
    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }
    }
}