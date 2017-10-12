using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePreview : MonoBehaviour {

    public string puzzleName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click()
    {
        Debug.Log("CLICK " + puzzleName);
        PlayMenu.instance.SetSelected(puzzleName, gameObject.transform.parent.gameObject);
        /*
        ImageManager.instance.StartCoroutine(PieceManager.instance.GetImage(puzzleName));
        PlayMenu.instance.gameObject.SetActive(false);
        SideSettingsMenu.instance.gameObject.SetActive(true);
        */
    }
}
