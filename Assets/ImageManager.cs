using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class ImageManager : MonoBehaviour {
    public GameObject thumbnailPrefab;
    public static readonly string[] imageNames = new string[] {
        "candy_eggs1",
        "dry_dock",
        "duck",
        "frog",
        "giraffe",
        "gummy_candy1",
        "harbor",
        "harbor1",
        "harbor2",
        "horse_landscape",
        "jelly_beans",
        "mountains"
    };

    public static ImageManager instance;
    // Use this for initialization
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {

    }
    
    IEnumerator UploadImage()
    {
        message += "Upload Image";
        
        yield return null;
    }


    private void PickImageFinished(ePickImageFinishReason _reason, Texture2D _image)
    {
        if(_reason == ePickImageFinishReason.SELECTED)
        {
            GameObject piece = GameObject.Find("Piece");
            piece.GetComponent<Renderer>().material.mainTexture = _image;
            GameObject pieceManager = GameObject.Find("PieceManager");
            pieceManager.GetComponent<PieceManager>().BuildPieces();
        }
        message += "" + _reason.ToString();
    }

    /*
    private void OnGUI() {
        if (GUI.Button(new Rect(10, 310, 300, 150), "Upload Image"))
        {
            // Set popover to last touch position
            NPBinding.UI.SetPopoverPointAtLastTouchPosition();

            // Pick image
            NPBinding.MediaLibrary.PickImage(eImageSource.ALBUM, 1.0f, PickImageFinished);
        }
        GUI.Label(new Rect(10, 475, 600, 600), message);
    }
    */
    string message = "";

    public IEnumerator LoadThumbnails()
    {
        AssetBundle myLoadedThumbsAssetBundle = null;
        AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
        for (int i = 0; i < bundles.Length; i++)
        {
            Debug.Log("Bundle: " + bundles[i].name);
            if (bundles[i].name == "thumbs.unity3d")
            {
                myLoadedThumbsAssetBundle = bundles[i];
            }
        }
        if (myLoadedThumbsAssetBundle == null)
        {
            while (!Caching.ready)
                yield return null;
            Caching.CleanCache();
            var www = WWW.LoadFromCacheOrDownload("https://s3-us-west-2.amazonaws.com/puzzle-tyrawr/images/thumbs.unity3d", 4);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield return null;
            }
            myLoadedThumbsAssetBundle = www.assetBundle;
        }

        // get parent for thumbs
        GameObject thumbsParent = GameObject.Find("Canvas/Play Menu/Scroll/Content");
        // load game objects
        GameObject[] thumbGameObjects = new GameObject[imageNames.Length];
        for(int i = 0; i < thumbGameObjects.Length; i++)
        {
            thumbGameObjects[i] = GameObject.Instantiate(thumbnailPrefab) as GameObject;
            thumbGameObjects[i].gameObject.transform.parent = thumbsParent.transform;
            Image img = thumbGameObjects[i].GetComponent<Image>();
            Texture2D tex = myLoadedThumbsAssetBundle.LoadAsset("Assets/Images/" + imageNames[i] + "_128.jpeg") as Texture2D;
            GameObject go = thumbGameObjects[i].gameObject;
            Material m = new Material(Shader.Find("UI/Default"));
            go.GetComponent<Image>().material = m;
            go.GetComponent<Image>().material.mainTexture = tex;
            go.GetComponent<PuzzlePreview>().puzzleName = imageNames[i];
        }
    }
}
