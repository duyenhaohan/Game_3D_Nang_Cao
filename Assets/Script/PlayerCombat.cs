using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;
    bool canAttack = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || !canAttack) return;

        animator.SetTrigger("Attack");
        StartCoroutine(Cooldown());
    }

    System.Collections.IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.6f);
        canAttack = true;
    }
}
