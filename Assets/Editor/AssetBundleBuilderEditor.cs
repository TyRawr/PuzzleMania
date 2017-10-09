using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilderEditor : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        var bundleBuild = new AssetBundleBuild[2]; 
        bundleBuild[0].assetBundleName = "images.unity3d";
        bundleBuild[1].assetBundleName = "thumbs.unity3d";
        var imagesNames = new string[ImageManager.imageNames.Length];
        var thumbNames = new string[ImageManager.imageNames.Length];
        for (int i = 0; i < ImageManager.imageNames.Length; i++) {
            string imgName = ImageManager.imageNames[i];
            imagesNames[i] = "Assets/Images/" + imgName + "_512.jpeg";
            thumbNames[i] = "Assets/Images/" + imgName + "_128.jpeg";
        }
        bundleBuild[0].assetNames = imagesNames;
        bundleBuild[1].assetNames = thumbNames;
        Debug.Log("build asset bundle");
        BuildPipeline.BuildAssetBundles("Assets/ABs", bundleBuild, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

}


