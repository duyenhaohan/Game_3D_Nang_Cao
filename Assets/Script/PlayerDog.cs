using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DogPlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float rotationSpeed = 10f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Fake Animation")]
    public float bobFrequency = 10f;
    public float bobAmount = 0.02f;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator anim;
    private Vector3 startLocalPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); // có thì dùng, không có cũng không sao
        startLocalPos = transform.localPosition;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // ===== INPUT =====
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // ===== CAMERA RELATIVE MOVE =====
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // ===== MOVE & ROTATE =====
        if (moveDir.magnitude > 0.1f)
        {
            controller.Move(moveDir * currentSpeed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
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

        // ===== FAKE ANIMATION (NHÚN NHẸ KHI CHẠY) =====
        if (controller.velocity.magnitude > 0.1f)
        {
            float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmount;
            transform.localPosition = startLocalPos + Vector3.up * bob;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startLocalPos,
                Time.deltaTime * 5f
            );
        }

        // ===== ANIMATOR (NẾU CÓ) =====
        if (anim != null)
        {
            float speedPercent = controller.velocity.magnitude / runSpeed;
            anim.SetFloat("Speed", speedPercent);
        }
    }
}
