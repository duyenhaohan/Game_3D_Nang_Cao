using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerDog : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float rotationSpeed = 10f;

    [Header("Gravity")]
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator anim;

    // Lưu hướng di chuyển cuối cùng
    private Vector3 lastMoveDirection = Vector3.forward;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // ===== MOVE & ROTATE =====
        if (inputDir.magnitude > 0.1f)
        {
            lastMoveDirection = inputDir;

            controller.Move(inputDir * currentSpeed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // Giữ hướng cuối cùng khi đứng yên
            Quaternion targetRot = Quaternion.LookRotation(lastMoveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        // ===== GRAVITY =====
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ===== ANIMATION: CHỈ RUN KHI DI CHUYỂN =====
        if (anim != null)
        {
            if (inputDir.magnitude > 0.1f)
            {
                float speedPercent = currentSpeed / runSpeed;
                anim.SetFloat("Speed", speedPercent);
            }
            else
            {
                anim.SetFloat("Speed", 0f); // Idle
            }
        }
    }
}
