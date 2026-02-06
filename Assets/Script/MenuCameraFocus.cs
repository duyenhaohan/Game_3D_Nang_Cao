using UnityEngine;

public class MenuCameraFocus : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 15f;

    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target);
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
