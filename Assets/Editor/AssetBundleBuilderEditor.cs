using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilderEditor : MonoBehaviour
{

    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        var bundleBuild = new AssetBundleBuild[1]; 
        bundleBuild[0].assetBundleName = "images.unity3d";
        var imagesNames = new string[2];
        imagesNames[0] = "Assets/Images/duck.jpeg";
        imagesNames[1] = "Assets/Images/giraffe.jpeg";
        bundleBuild[0].assetNames = imagesNames;


        Debug.Log("build asset bundle");
        BuildPipeline.BuildAssetBundles("Assets/ABs", bundleBuild, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

}


