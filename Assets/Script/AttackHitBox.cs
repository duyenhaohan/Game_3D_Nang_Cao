using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;
    public string targetTag;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        if (targetTag == "Enemy")
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);

        if (targetTag == "Player")
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }
}
