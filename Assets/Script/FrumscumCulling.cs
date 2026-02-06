using UnityEngine;

public class FrustumCulling : MonoBehaviour
{
    Renderer rend;
    Camera cam;
    Plane[] planes;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        cam = Camera.main;
    }

    void Update()
    {
        if (!rend || !cam) return;

        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        bool visible = GeometryUtility.TestPlanesAABB(
            planes,
            rend.bounds
        );

        rend.enabled = visible;
    }
}
