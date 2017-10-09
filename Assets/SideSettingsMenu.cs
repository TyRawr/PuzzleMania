using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSettingsMenu : MonoBehaviour {
    public static SideSettingsMenu instance;
	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Back()
    {
        PlayMenu.instance.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void Reset()
    {
        PieceManager.instance.ResetPieces();
    }
}
