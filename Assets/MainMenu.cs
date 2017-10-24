using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public GameObject gameboard;
    public GameObject piecesParent;
    public SideSettingsMenu sideSettingMenu;
    public PlayMenu playMenu;
    public SetupMenu setupMenu;
    public CameraMenu cameraMenu;

    public static MainMenu instance;
	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {

    }

    public void Settings()
    {

    }

    public void Store()
    {

    }

    public void Back()
    {
        playMenu.gameObject.SetActive(true);
        sideSettingMenu.gameObject.SetActive(false);
    }
}
