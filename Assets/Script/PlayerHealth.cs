using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 100;
    public int currentHP;
    
    [Header("Contact Damage")]
    public int contactDamage = 10;
    public float contactDamageCooldown = 0.5f;
    
    [Header("UI")]
    public Slider healthBar;           // Thanh máu trên màn hình
    public Text healthText;            // Text hiển thị số máu
    
    [Header("Effects")]
    public ParticleSystem hitEffect;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    
    private Animator animator;
    private AudioSource audioSource;
    private bool isDead;
    private float lastContactDamageTime;
    private PlayerController playerController;
    private Renderer playerRenderer;

    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        playerRenderer = GetComponentInChildren<Renderer>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // KIỂM TRA VÀ KHỞI TẠO THANH MÁU
        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = currentHP;
            Debug.Log($"✅ HealthBar khởi tạo: max={healthBar.maxValue}, value={healthBar.value}");
        }
        else
        {
            Debug.LogError("❌ HealthBar chưa được gán trong Inspector!");
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
        
        Debug.Log($"💔 Player nhận {dmg} sát thương. HP: {currentHP}/{maxHP}");

        if (animator != null)
            animator.SetTrigger("Hit");

        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        if (hitEffect != null)
            hitEffect.Play();

        StartCoroutine(FlashRed());
        
        // CẬP NHẬT THANH MÁU
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        // CẬP NHẬT SLIDER
        if (healthBar != null)
        {
            healthBar.value = currentHP;
            Debug.Log($"📊 HealthBar cập nhật: {healthBar.value}/{healthBar.maxValue}");
        }
        
        // CẬP NHẬT TEXT
        if (healthText != null)
        {
            healthText.text = $"{currentHP}/{maxHP}";
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        if (playerRenderer != null)
        {
            Color originalColor = playerRenderer.material.color;
            playerRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            playerRenderer.material.color = originalColor;
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
        
        Debug.Log($"💚 Player hồi {amount} máu. HP: {currentHP}/{maxHP}");
        
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
        
        Debug.Log("💀 Player đã chết!");
        
        if (animator != null)
            animator.SetTrigger("Die");

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (playerController != null)
            playerController.enabled = false;

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
        SceneManager.LoadScene("GameOver");
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealthPercent()
    {
        return (float)currentHP / maxHP;
    }
}