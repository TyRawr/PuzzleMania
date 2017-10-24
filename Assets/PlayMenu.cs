using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;

public class PlayMenu : MonoBehaviour {
    public static PlayMenu instance;
    public GameObject ContentParent;
    public Button PlayButton;
    public Texture2D fallbackCameraImg;
    private string puzzleId;
	// Use this for initialization
	void Start () {
        instance = this;
        GameObject imageManagerGameObject = GameObject.Find("Main Camera");
        ImageManager imageManager = imageManagerGameObject.GetComponent<ImageManager>();
        StartCoroutine(imageManager.LoadThumbnails());
        PlayButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P) || Input.touchCount > 2)
        {
            PickImageFinished(ePickImageFinishReason.SELECTED, fallbackCameraImg);
        }
	}

    private void PickImageFinished(ePickImageFinishReason _reason, Texture2D _image)
    {
        if (_reason == ePickImageFinishReason.SELECTED)
        {
            MainMenu.instance.cameraMenu.gameObject.SetActive(true);
            MainMenu.instance.cameraMenu.PickImageFinished(_reason, _image);
            
            PlayMenu.instance.gameObject.SetActive(false);
            /*
            GameObject piece = GameObject.Find("Piece");
            piece.GetComponent<Renderer>().material.mainTexture = _image;
            GameObject pieceManager = GameObject.Find("PieceManager");
            pieceManager.GetComponent<PieceManager>().BuildPieces();
            
            //SideSettingsMenu.instance.gameObject.SetActive(true);
            */
        }
    }

    public void Custom()
    {
        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        // Pick image
        NPBinding.MediaLibrary.PickImage(eImageSource.ALBUM, 1.0f, PickImageFinished);
    }

    public void SetSelected(string selectedID,GameObject selectedObject)
    {
        puzzleId = selectedID;
        for(int i = 0; i < ContentParent.transform.childCount; i++)
        {
            var child = ContentParent.transform.GetChild(i);
            var highlight = child.transform.GetChild(0);
            highlight.gameObject.SetActive(false);
        }
        Debug.Log("puzzleId " + selectedID);
        selectedObject.transform.GetChild(0).gameObject.SetActive(true);
        PlayButton.interactable = true;
    }

    public void Play()
    {
        PlayMenu.instance.gameObject.SetActive(false);
        MainMenu.instance.setupMenu.gameObject.SetActive(true);
        MainMenu.instance.setupMenu.Setup(puzzleId);
    }
}
