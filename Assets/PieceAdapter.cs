using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAdapter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}
    private void OnTriggerEnter(Collider other)
    {
        GameObject[] ret = new GameObject[2];
        ret[0] = this.gameObject;
        ret[1] = other.gameObject;
        SendMessageUpwards("AdapterCollision", (object)ret);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject[] ret = new GameObject[2];
        ret[0] = this.gameObject;
        ret[1] = other.gameObject;
        this.transform.parent.SendMessage("AdapterCollisionExit", (object)ret);
    }
}
