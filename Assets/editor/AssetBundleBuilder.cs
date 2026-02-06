using UnityEditor;
using UnityEngine;
using System.IO;

public class AssetBundleBuilder
{
    [MenuItem("Tools/AssetBundle/Build All")]
    public static void BuildAll()
    {
        string outputPath = "Assets/StreamingAssets/Bundles";

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(
            outputPath,
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget
        );

        Debug.Log("✅ Build AssetBundle xong");
    }
}