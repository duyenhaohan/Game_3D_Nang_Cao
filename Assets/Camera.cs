using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
