using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    public Camera mainCamera;
    public Behaviour[] disableWhenInvisible;

    Renderer rend;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (rend == null) return;

        bool visible = IsVisibleFrom(rend, mainCamera);

        foreach (var b in disableWhenInvisible)
            b.enabled = visible;
    }

    bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
