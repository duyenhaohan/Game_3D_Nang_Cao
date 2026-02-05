using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -20f;

    [Header("Combat")]
    public int attackDamage = 20;
    public Collider attackHitbox;

    CharacterController controller;
    Animator animator;

    Vector2 moveInput;
    Vector3 velocity;

    bool isDead;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        velocity.y = -2f; // GIỮ CHÂN DÍNH ĐẤT
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
        if (!context.started) return;

        if (controller.isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (!context.performed) return;

        animator.SetTrigger("Attack");
    }

    // ================= HITBOX (Animation Event) =================

    public void EnableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }

    // ================= UPDATE =================

    void Update()
    {
        if (isDead) return;

        // MOVE THEO HƯỚNG PLAYER XOAY
        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // GRAVITY CHUẨN
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", moveInput.magnitude);
    }

    // ================= CALLED BY PlayerHealth =================
    public void SetDead()
    {
        isDead = true;
    }
}