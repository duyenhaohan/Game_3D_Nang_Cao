using UnityEngine;

[RequireComponent(typeof(LODGroup))]
public class LODMeshOptimize : MonoBehaviour
{
    public Mesh highMesh;
    public Mesh mediumMesh;
    public Mesh lowMesh;

    void Start()
    {
        LODGroup lodGroup = GetComponent<LODGroup>();

        Renderer r0 = CreateLOD("LOD0", highMesh);
        Renderer r1 = CreateLOD("LOD1", mediumMesh);
        Renderer r2 = CreateLOD("LOD2", lowMesh);

        LOD[] lods = new LOD[3];
        lods[0] = new LOD(0.6f, new Renderer[] { r0 });
        lods[1] = new LOD(0.3f, new Renderer[] { r1 });
        lods[2] = new LOD(0.1f, new Renderer[] { r2 });

        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();
    }

    Renderer CreateLOD(string name, Mesh mesh)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();

        mf.sharedMesh = OptimizeMesh(mesh);
        mr.sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;

        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;

        return mr;
    }

    Mesh OptimizeMesh(Mesh mesh)
    {
        Mesh m = Instantiate(mesh);

        // Unity 6.x chỉ hỗ trợ hàm này
        m.OptimizeReorderVertexBuffer();
        m.Optimize();

        return m;
    }
}
