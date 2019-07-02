using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenu : MonoBehaviour {

    public static SetupMenu instance;

    public NumberOfPieces numberOfPieces;
    public Image thumbnail;
    private string puzzleId;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Back()
    {
        SetupMenu.instance.gameObject.SetActive(false);
        MainMenu.instance.playMenu.gameObject.SetActive(true);
    }
    Texture originalImage;

    public void SetupFromCamera()
    {
        this.puzzleId = puzzleId;
        Material m = new Material(Shader.Find("UI/Default"));
        thumbnail.GetComponent<Image>().material = m;
        Texture2D tex = ImageManager.texture;
        thumbnail.material.mainTexture = tex;
        Texture2D originalSizedImage = null;

    }

    public void Setup(string puzzleId = "")
    {
        this.puzzleId = puzzleId;
        Material m = new Material(Shader.Find("UI/Default"));
        thumbnail.GetComponent<Image>().material = m;
        Texture2D tex =  ImageManager.instance.GetThumbnail(puzzleId);
        thumbnail.material.mainTexture = tex;
        Texture2D originalSizedImage = null;
        ImageManager.instance.StartCoroutine(ImageManager.instance.LoadTexture(puzzleId));
    }

    public void Play()
    {
        int numPieces = numberOfPieces.GetNumberOfPieces();
        PieceManager.instance.SetColsAndRows(numPieces);
        MainMenu.instance.sideSettingMenu.gameObject.SetActive(true);
        SetupMenu.instance.gameObject.SetActive(false);
        //ImageManager.instance.StartCoroutine(PieceManager.instance.GetImage(puzzleId));
        GameObject piece = GameObject.Find("Piece"); 
        piece.GetComponent<Renderer>().material.mainTexture = ImageManager.texture;
        GameObject gameBoard = GameObject.Find("Gameboard");
        gameBoard.transform.position = Vector3.zero;
        PieceManager.instance.BuildPieces();
        //PieceManager.instance.ResetPieces();
    }
}
