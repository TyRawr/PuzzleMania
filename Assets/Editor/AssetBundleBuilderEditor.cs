using System.Collections;
using System;
using System.IO;
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
        var imagesNames = new string[61];
        var thumbNames = new string[61];
        for (int i = 0; i < 61; i++) {
            string imgName = "file_" + i + ".jpeg";
            imagesNames[i] = "Assets/Images/originals/" + imgName;
            thumbNames[i] = "Assets/Images/thumbs/" + imgName;
        }
        bundleBuild[0].assetNames = imagesNames;
        bundleBuild[1].assetNames = thumbNames;
        Debug.Log("build asset bundle");
        BuildPipeline.BuildAssetBundles("Assets/ABs", bundleBuild, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

}


