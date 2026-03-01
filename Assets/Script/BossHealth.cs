using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 500; // Lớn hơn enemy thường
    public int currentHP;
    
    [Header("UI")]
    public Slider healthBar; // Thanh máu trên đầu boss
    public Text healthText;  // Text hiển thị số máu
    
    [Header("Effects")]
    public ParticleSystem hitEffect;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    
    private Animator animator;
    private AudioSource audioSource;
    private bool isDead;
    private BossController bossController;
    
    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        bossController = GetComponent<BossController>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = currentHP;
        }
        
        if (healthText != null)
            healthText.text = $"{currentHP}/{maxHP}";
    }
    
    public void TakeDamage(int dmg)
    {
        if (isDead) return;
        
        currentHP -= dmg;
        currentHP = Mathf.Max(0, currentHP);
        
        Debug.Log($"👹 Boss nhận {dmg} sát thương. HP: {currentHP}/{maxHP}");
        
        // Animation bị đánh
        if (bossController != null)
            bossController.TakeHit();
        
        // Âm thanh
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
        
        // Hiệu ứng
        if (hitEffect != null)
            hitEffect.Play();
        
        // Cập nhật UI
        UpdateHealthBar();
        
        if (currentHP <= 0)
            Die();
    }
    
    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = currentHP;
        
        if (healthText != null)
            healthText.text = $"{currentHP}/{maxHP}";
    }
    
    void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        if (bossController != null)
            bossController.Die();
        
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
        
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);
        
        if (healthText != null)
            healthText.gameObject.SetActive(false);
        
        Debug.Log("💀 Boss đã chết!");
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