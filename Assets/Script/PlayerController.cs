using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -20f;

    CharacterController controller;
    Animator animator;

    Vector2 moveInput;
    Vector3 velocity;
    bool isDead;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocity.y = -2f;
    }

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
        // ❌ KHÔNG bật hitbox ở đây
    }
public AttackHitbox swordHitbox;

public void EnableHitbox()
{
    if (swordHitbox != null)
        swordHitbox.EnableHitbox();
}

public void DisableHitbox()
{
    if (swordHitbox != null)
        swordHitbox.DisableHitbox();
}
    void Update()
    {
        if (isDead) return;

        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", moveInput.magnitude);
    }

    public void SetDead()
    {
        isDead = true;
    }
}