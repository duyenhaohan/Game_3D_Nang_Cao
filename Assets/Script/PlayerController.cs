using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    [Header("Combat")]
    public int maxHP = 100;
    public int attackDamage = 20;

    CharacterController controller;
    Animator animator;

    Vector2 moveInput;
    Vector3 velocity;

    int currentHP;
    bool isDead;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    // ================= INPUT =================

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (!context.performed) return;

        if (controller.isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    // 👉 THÊM MỚI: ATTACK
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (!context.performed) return;

        animator.SetTrigger("Attack");
    }
    public Collider attackHitbox;

public void EnableHitbox()
{
    attackHitbox.enabled = true;
}

public void DisableHitbox()
{
    attackHitbox.enabled = false;
}

    // ================= UPDATE =================

    void Update()
    {
        if (isDead) return;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.sqrMagnitude > 0.01f)
            transform.forward = move;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animator
        animator.SetFloat("Speed", move.magnitude);
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    // ================= HP / DIE =================

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);

        moveInput = Vector2.zero;
        controller.enabled = false;
    }
}
