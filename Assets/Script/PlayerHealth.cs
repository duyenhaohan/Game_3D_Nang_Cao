using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // THÊM DÒNG NÀY

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int contactDamage = 10;
    public float contactDamageCooldown = 0.5f;

    [Header("UI")]
    public Slider healthBar; // Kéo thanh slider vào đây
    public Text healthText; // Tùy chọn: hiện số máu

    Animator animator;
    bool isDead;
    float lastContactDamageTime;

    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Khởi tạo thanh máu
        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = currentHP;
        }
        
        if (healthText != null)
        {
            healthText.text = $"{currentHP}/{maxHP}";
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;
        currentHP = Mathf.Max(0, currentHP);
        
        Debug.Log($"Player HP: {currentHP}");
        
        // Cập nhật UI
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHP;
        }
        
        if (healthText != null)
        {
            healthText.text = $"{currentHP}/{maxHP}";
        }
    }

    // Thêm hàm heal nếu cần
    public void Heal(int amount)
    {
        if (isDead) return;
        
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
        UpdateHealthBar();
    }

    void OnTriggerEnter(Collider other)
    {
        TryContactDamage(other.gameObject);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        TryContactDamage(hit.gameObject);
    }

    void TryContactDamage(GameObject other)
    {
        if (isDead) return;
        if (!other.CompareTag("Enemy")) return;
        if (Time.time - lastContactDamageTime < contactDamageCooldown) return;

        lastContactDamageTime = Time.time;
        TakeDamage(contactDamage);
    }

    void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        Debug.Log("💀 Player died!");
        
        if (animator != null)
            animator.SetTrigger("Die");

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        EnemyAttack[] enemyAttacks = FindObjectsOfType<EnemyAttack>();
        foreach (EnemyAttack attack in enemyAttacks)
        {
            attack.enabled = false;
        }

        Invoke("LoadGameOverScene", 2f);
    }

    void LoadGameOverScene()
    {
        Debug.Log("📱 Loading GameOver scene...");
        SceneManager.LoadScene("GameOver");
    }
    
    public bool IsDead()
    {
        return isDead;
    }
}