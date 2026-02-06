using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{
    public string bundleName = "player";
    public string materialName = "Sword_Red";
    public MeshRenderer swordRenderer;

    void Start()
    {
        string path = Application.streamingAssetsPath + "/Bundles/" + bundleName;
        AssetBundle bundle = AssetBundle.LoadFromFile(path);

        if (bundle == null)
        {
            Debug.LogError("❌ Load AssetBundle fail");
            return;
        }

        Material mat = bundle.LoadAsset<Material>(materialName);
        if (mat != null)
        {
            swordRenderer.material = mat;
        }

        bundle.Unload(false);
    }
}