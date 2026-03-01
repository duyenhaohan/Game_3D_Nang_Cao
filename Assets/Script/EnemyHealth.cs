using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Heal")]
    public float healSpeed = 20f;
    
    [Header("UI")]
    public EnemyHealthBar healthBarUI;
    
    [Header("Effects")]
    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    private Animator animator;
    private EnemyAI ai;
    private NavMeshAgent agent;
    private Collider col;
    private AudioSource audioSource;
    private bool isDead;
    private Renderer enemyRenderer;

    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        ai = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        if (healthBarUI != null)
            healthBarUI.UpdateHealth(currentHP, maxHP);
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (animator != null)
            animator.SetTrigger("Hit");

        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        if (hitEffect != null)
            hitEffect.Play();

        StartCoroutine(FlashRed());
        
        if (healthBarUI != null)
            healthBarUI.UpdateHealth(currentHP, maxHP);

        if (currentHP <= 0)
            Die();
    }

    System.Collections.IEnumerator FlashRed()
    {
        if (enemyRenderer != null)
        {
            Color originalColor = enemyRenderer.material.color;
            enemyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            enemyRenderer.material.color = originalColor;
        }
    }

    public void HealOverTime()
    {
        if (isDead) return;

        currentHP += Mathf.RoundToInt(healSpeed * Time.deltaTime);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        
        if (healthBarUI != null)
            healthBarUI.UpdateHealth(currentHP, maxHP);
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetBool("IsDead", true);
            

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (ai != null) ai.enabled = false;
        if (agent != null) agent.isStopped = true;
        if (col != null) col.enabled = false;
        
        if (healthBarUI != null)
            healthBarUI.gameObject.SetActive(false);

        AttackHitbox[] hitboxes = GetComponentsInChildren<AttackHitbox>();
        foreach (AttackHitbox hitbox in hitboxes)
            hitbox.enabled = false;

        Destroy(gameObject, 3f);
    }

    public float GetHPPercent()
    {
        return (float)currentHP / maxHP;
    }

    public bool IsDead()
    {
        return isDead;
    }
}