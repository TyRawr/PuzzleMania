using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour {

    public int col;
	// Use this for initialization
	void Start () {
        RectTransform parent = transform.parent.gameObject.GetComponent<RectTransform>();
        GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(parent.rect.height / col, parent.rect.height / col);
	}
	
}
