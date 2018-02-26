using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportUserAudio : MonoBehaviour {
    [UnityEditor.MenuItem("Assets/Build AssetBundle")]
    static void ExportResource()
    {
        string filePath = "Assets/StreamingAssets/UserMusic";

        UnityEditor.BuildPipeline.BuildAssetBundles(filePath, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.StandaloneWindows);
    }
}
