using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Transform enemyTransform;
    public Vector3 offset = new Vector3(0, 2.5f, 0);
    
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (healthBar == null)
            healthBar = GetComponentInChildren<Slider>();
            
        if (enemyTransform == null)
            enemyTransform = transform.parent;
    }
    
    void LateUpdate()
    {
        if (mainCamera == null) 
        {
            mainCamera = Camera.main;
            return;
        }
        
        if (enemyTransform != null && healthBar != null)
        {
            transform.rotation = mainCamera.transform.rotation;
            transform.position = enemyTransform.position + offset;
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