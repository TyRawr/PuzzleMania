using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class ImageManager : MonoBehaviour {

    // Use this for initialization
    void Start() {
        
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
}
