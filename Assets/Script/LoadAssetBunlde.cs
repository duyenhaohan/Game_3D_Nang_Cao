using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{
    public string bundleName = "player";
    public string swordmaterialName = "Sword_Red";
    public string shieldmaterialName = "Shield_Red";
    public string hairmaterialName = "Hair_Red";
      
    public MeshRenderer swordRenderer;
    public MeshRenderer shieldRenderer;
    public MeshRenderer hairRenderer;

    void Start()
    {
        string path = Application.streamingAssetsPath + "/Bundles/" + bundleName;
        AssetBundle bundle = AssetBundle.LoadFromFile(path);

        if (bundle == null)
        {
            Debug.LogError("❌ Load AssetBundle fail");
            return;
        }

        Material swordmat = bundle.LoadAsset<Material>(swordmaterialName);
        if (swordmat != null)
        {
            swordRenderer.material = swordmat;
        }
        Material shieldmat = bundle.LoadAsset<Material>(shieldmaterialName);
        if (shieldmat != null)
        {
            shieldRenderer.material = shieldmat;
        }
         Material hairmat = bundle.LoadAsset<Material>(hairmaterialName);
        if (hairmat != null)
        {
            hairRenderer.material = hairmat;
        }


        bundle.Unload(false);
    }
}