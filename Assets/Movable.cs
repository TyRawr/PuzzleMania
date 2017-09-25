using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
    bool m_held, m_movedThisFrame, m_heldInitial;
    private Camera camera;
    Vector3 offset;
    // Use this for initialization
    void Start () {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        m_movedThisFrame = false;
        if (m_held)
        {
            Vector3 touchPos = Vector3.zero;
            
            
            if (Input.touchCount > 0)
            {
                touchPos = Input.GetTouch(0).position;
            }
            else
            {
                touchPos = Input.mousePosition;
            }
            touchPos = camera.ScreenToWorldPoint(touchPos);
            if (!m_heldInitial)
            {
                // calculate offset
                m_heldInitial = true;
                offset = touchPos - transform.position;
                offset = new Vector3(offset.x, offset.y, 0f);
            }
            
            Vector3 pos = new Vector3(touchPos.x, touchPos.y, 0f);

            this.transform.position = pos - offset;
        }
    }

    void OnMouseDown()
    {
        m_held = true;
        m_heldInitial = false;
    }

    void OnMouseUp()
    {
        m_held = false;
        m_heldInitial = false;
    }
}
