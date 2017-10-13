using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfPieces : MonoBehaviour {

    public Slider slider;
    private Text text;
    private float value;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        float value = slider.value * slider.value;
        this.value = slider.value;
        text.text = value.ToString() + " Pieces";
	}

    public int GetNumberOfPieces() 
    {
        return (int)value;
    }
}
