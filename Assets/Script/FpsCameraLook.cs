using UnityEngine;
using UnityEngine.InputSystem;



public class FirstPersonLook : MonoBehaviour
{
    public Transform playerBody;
    public float sensitivity = 0.1f;

    Vector2 lookInput;
    float xRotation;

    void Start()
    {
        // 👉 XÓA hoặc COMMENT 2 dòng này để không ẩn chuột
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        
        // Có thể thêm dòng này để đảm bảo chuột luôn hiện
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // Xoay lên xuống (CameraPivot)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // XOAY PLAYER (QUAN TRỌNG)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}