using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class ImageManager : MonoBehaviour {
    public static ImageManager instance;
    public static Texture2D texture;
    public GameObject thumbnailPrefab;

    
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

    public Texture2D GetThumbnail(string fileId)
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
        Texture2D tex = myLoadedThumbsAssetBundle.LoadAsset("Assets/Images/thumbs/file_" + fileId + ".jpeg") as Texture2D;
        return tex;
    }

    public IEnumerator LoadThumbnails()
    {
        AssetBundle myLoadedThumbsAssetBundle = null;
        AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
        Caching.ClearCache();
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
        GameObject[] thumbGameObjects = new GameObject[61];
        for(int i = 0; i < thumbGameObjects.Length; i++)
        {
            thumbGameObjects[i] = GameObject.Instantiate(thumbnailPrefab) as GameObject;
            thumbGameObjects[i].gameObject.transform.SetParent(thumbsParent.transform,false);
            thumbGameObjects[i].transform.localScale = Vector3.one;
            Image img = thumbGameObjects[i].GetComponent<Image>();
            Texture2D tex = myLoadedThumbsAssetBundle.LoadAsset("Assets/Images/thumbs/file_" + i + ".jpeg") as Texture2D;
            GameObject go = thumbGameObjects[i].transform.GetChild(1).gameObject;
            Material m = new Material(Shader.Find("UI/Default"));
            go.GetComponent<Image>().material = m;
            if(tex == null)
            {
                go.GetComponent<Image>().material.color = Color.black;
            }
            go.GetComponent<Image>().material.mainTexture = tex;
            go.GetComponent<PuzzlePreview>().puzzleName = i.ToString();
        }
    }

    /// <summary>
    /// Loads the static texture (ImageManager.texture) from AWS, given the file name, of the form ("file_#.jpeg").
    /// </summary>
    /// <param name="imgNumber"></param>
    /// <returns></returns>
    public IEnumerator LoadTexture(string imgNumber)
    {
        while (!Caching.ready)
            yield return null;
        Caching.ClearCache();
        var www = new WWW("https://s3-us-west-2.amazonaws.com/puzzle-tyrawr/images/originals/file_" + imgNumber + ".jpeg" ); // all files are jpeg
        ImageManager.texture = new Texture2D(4, 4, TextureFormat.DXT1, false);
        //WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(ImageManager.texture);
    }
}
