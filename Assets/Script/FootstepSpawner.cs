using UnityEngine;

public class FootstepSpawner : MonoBehaviour
{
    public GameObject footstepPrefab;   // Prefab bước chân
    public float stepDistance = 1.2f;    // Khoảng cách giữa các bước
    public LayerMask groundLayer;        // Layer mặt đất

    private Vector3 lastStepPosition;

    void Start()
    {
        lastStepPosition = transform.position;
    }

    void Update()
    {
        // Kiểm tra player có di chuyển không
        if (Vector3.Distance(transform.position, lastStepPosition) >= stepDistance)
        {
            SpawnFootstep();
            lastStepPosition = transform.position;
        }
    }

    void SpawnFootstep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
            Vector3 spawnPos = hit.point;
            spawnPos.y += 0.01f; // tránh bị chìm xuống đất

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Instantiate(footstepPrefab, spawnPos, rotation);
        }
    }
}
