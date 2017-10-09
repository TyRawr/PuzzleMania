using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class PlayMenu : MonoBehaviour {
    public static PlayMenu instance;
	// Use this for initialization
	void Start () {
        instance = this;
        GameObject imageManagerGameObject = GameObject.Find("Main Camera");
        ImageManager imageManager = imageManagerGameObject.GetComponent<ImageManager>();
        StartCoroutine(imageManager.LoadThumbnails());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PickImageFinished(ePickImageFinishReason _reason, Texture2D _image)
    {
        if (_reason == ePickImageFinishReason.SELECTED)
        {
            GameObject piece = GameObject.Find("Piece");
            piece.GetComponent<Renderer>().material.mainTexture = _image;
            GameObject pieceManager = GameObject.Find("PieceManager");
            pieceManager.GetComponent<PieceManager>().BuildPieces();
            PlayMenu.instance.gameObject.SetActive(false);
            SideSettingsMenu.instance.gameObject.SetActive(true);
        }
    }

    public void Custom()
    {
        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        // Pick image
        NPBinding.MediaLibrary.PickImage(eImageSource.ALBUM, 1.0f, PickImageFinished);
    }
}
