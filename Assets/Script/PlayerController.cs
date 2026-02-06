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
    public float attackStartDelay = 0.1f;
    public float attackActiveTime = 0.2f;

    CharacterController controller;
    Animator animator;

    Vector2 moveInput;
    Vector3 velocity;

    bool isDead;
    bool isAttacking;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        velocity.y = -2f; // GI? ch�n d�nh d?t
        // PlayerController.cs - Th�m Awake d? debug

        // Debug attackHitbox
        if (attackHitbox == null)
        {
            Debug.LogError("AttackHitbox not assigned in Inspector!");
        }
        else
        {
            Debug.Log($"AttackHitbox assigned: {attackHitbox.name}");
            attackHitbox.enabled = false; // �?m b?o t?t l�c d?u
            attackHitbox.isTrigger = true;

            AttackHitbox hitbox = attackHitbox.GetComponent<AttackHitbox>();
            if (hitbox != null)
            {
                hitbox.damage = attackDamage;
                hitbox.targetTag = "Enemy";
            }
            else
            {
                Debug.LogError("AttackHitbox missing AttackHitbox component!");
            }

            Rigidbody rb = attackHitbox.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = attackHitbox.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        velocity.y = -2f;
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
        if (!isAttacking)
            StartCoroutine(AttackWindow());
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

    System.Collections.IEnumerator AttackWindow()
    {
        isAttacking = true;
        if (attackStartDelay > 0f)
            yield return new WaitForSeconds(attackStartDelay);
        EnableHitbox();
        if (attackActiveTime > 0f)
            yield return new WaitForSeconds(attackActiveTime);
        DisableHitbox();
        isAttacking = false;
    }

    // ================= UPDATE =================

    void Update()
    {
        if (isDead) return;

        // MOVE THEO HU?NG PLAYER XOAY
        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // GRAVITY CHU?N
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
