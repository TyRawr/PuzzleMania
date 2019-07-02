using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
using VoxelBusters;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;
*/

public class CameraMenu : MonoBehaviour {
    public static CameraMenu instance;

    public Texture2D texture;
    public GameObject imageObject;
    public Cropper cropper;

    // Use this for initialization
    void Start () {
        instance = this;
    }

    void Update()
    {

    }

    public void Play()
    {
        texture = cropper.GetCroppedImage();
        if(texture == null)
        {
            Debug.LogError("Tecture is null :(");
        }
        ImageManager.texture = texture;
        /*
        GameObject piece = GameObject.Find("Piece");
        piece.GetComponent<Renderer>().material.mainTexture = texture;
        GameObject pieceManager = GameObject.Find("PieceManager");
        pieceManager.GetComponent<PieceManager>().BuildPieces();
        PlayMenu.instance.gameObject.SetActive(false);
        */
        CameraMenu.instance.gameObject.SetActive(false);
        MainMenu.instance.setupMenu.gameObject.SetActive(true);
        MainMenu.instance.setupMenu.SetupFromCamera();
    }

    /*
    public void PickImageFinished(ePickImageFinishReason _reason, Texture2D _image)
    {
        if (_reason == ePickImageFinishReason.SELECTED)
        {
            // get width, height
            int width = _image.width;
            int height = _image.height;

            float aspectRatio = (float)width / (float)height;
            float scaleWidth, scaleHeight;
            scaleHeight = 5f;
            scaleWidth = scaleHeight * aspectRatio;

            MeshRenderer imgRenderer = imageObject.GetComponent<MeshRenderer>();
            imgRenderer.material.mainTexture = _image;
            imgRenderer.gameObject.transform.localScale = new Vector3(scaleWidth, scaleHeight, 1f);
            //SideSettingsMenu.instance.gameObject.SetActive(true);
        }
    }
    */

    public void OnEnable()
    {
        imageObject.SetActive(true);
        cropper.gameObject.SetActive(true);
        MainMenu.instance.gameboard.SetActive(false);
        MainMenu.instance.piecesParent.SetActive(false);
    }

    public void OnDisable()
    {
        imageObject.SetActive(false);
        cropper.gameObject.SetActive(false);
        MainMenu.instance.gameboard.SetActive(true);
        MainMenu.instance.piecesParent.SetActive(true);
    }
}
