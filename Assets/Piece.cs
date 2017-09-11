using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    public int row;
    public int col;
    public PieceManager.EDGETYPE up, right, down, left;
    public Piece upPiece, rightPiece, leftPiece, downPiece;
    public PieceAdapter m_topAdapter, m_rightAdapter, m_bottomAdapter, m_leftAdapter;

    public bool m_held = false;
    public List<Piece> m_potentialMatches = new List<Piece>();
    public List<Piece> m_successfulMatches = new List<Piece>();
    private Dictionary<PieceAdapter, PieceAdapter> m_pieceAdapterDictionary = new Dictionary<PieceAdapter, PieceAdapter>();

    private Camera camera;
    
	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_topAdapter = transform.GetChild(0).GetComponent<PieceAdapter>();
        m_rightAdapter = transform.GetChild(1).GetComponent<PieceAdapter>();
        m_bottomAdapter = transform.GetChild(2).GetComponent<PieceAdapter>();
        m_leftAdapter = transform.GetChild(3).GetComponent<PieceAdapter>();
    }

    public PieceAdapter GetTopAdapter()
    {
        return m_topAdapter;
    }

    public PieceAdapter GetRightAdapter()
    {
        return m_rightAdapter;
    }

    public PieceAdapter GetBottomAdapter()
    {
        return m_bottomAdapter;
    }

    public PieceAdapter GetLeftAdapter()
    {
        return m_leftAdapter;
    }
    private bool m_movedThisFrame = false;
    // Update is called once per frame
    void Update () {
        m_movedThisFrame = false;
        if(m_held)
        {
            Vector3 touchPos = Vector3.zero;
            if(Input.touchCount > 0)
            {
                touchPos = Input.GetTouch(0).position;
            } else
            {
                touchPos = Input.mousePosition;
            }
            touchPos = camera.ScreenToWorldPoint(touchPos);
            Vector3 pos = new Vector3(touchPos.x, touchPos.y, 0f);

            
            
            this.transform.position = pos;
            //everything I am attached to, move
            StartAlignment();
        }
    }

    private void StartAlignment()
    {
        foreach (Piece p in m_successfulMatches)
        {
            if (p == upPiece)
                p.Align(transform, Direction.UP);
            if (p == rightPiece)
                p.Align(transform, Direction.RIGHT);
            if (p == downPiece)
                p.Align(transform, Direction.DOWN);
            if (p == leftPiece)
                p.Align(transform, Direction.LEFT);
        }
    }
    

    private void AdapterCollisionExit(object obj)
    {
        GameObject[] gos = (GameObject[])obj;
        Debug.Log("AdapterCollisionExit " + gos[0].transform.parent.name + "  " + gos[1].transform.parent.name);
        GameObject thisGO = gos[0];
        GameObject otherGO = gos[1];

        PieceAdapter thisAdapter = thisGO.GetComponent<PieceAdapter>();
        PieceAdapter otherAdapter = otherGO.GetComponent<PieceAdapter>();

        Piece otherPiece = otherGO.transform.parent.GetComponent<Piece>();
        if (upPiece != null &&
            otherPiece == upPiece &&
            otherPiece.GetBottomAdapter() == otherAdapter)
        {
            m_potentialMatches.Remove(otherPiece);
        }

        //if right piece not null and other piece is this pieces up piece and that the adapters match.
        if (rightPiece != null &&
        otherPiece == rightPiece &&
        otherPiece.GetLeftAdapter() == otherAdapter)
        {
            m_potentialMatches.Remove(otherPiece);
        }

        //if bottom piece not null and other piece is this pieces up piece and that the adapters match.
        if (downPiece != null &&
        otherPiece == downPiece &&
        otherPiece.GetTopAdapter() == otherAdapter)
        {
            m_potentialMatches.Remove(otherPiece);
        }

        //if left piece not null and other piece is this pieces up piece and that the adapters match.
        if (leftPiece != null &&
        otherPiece == leftPiece &&
        otherPiece.GetRightAdapter() == otherAdapter)
        {
            m_potentialMatches.Remove(otherPiece);
        }
    }

    private void AdapterCollision(object obj)
    {
        GameObject[] gos = (GameObject[])obj;
        if(true)
        {
            Debug.Log("AdapterCollision " + gos[0].transform.parent.name + "  " + gos[1].transform.parent.name);

            /// assemble sucessful matches.
            GameObject thisGO = gos[0];
            GameObject otherGO = gos[1];

            PieceAdapter thisAdapter = thisGO.GetComponent<PieceAdapter>();
            PieceAdapter otherAdapter = otherGO.GetComponent<PieceAdapter>();

            Piece otherPiece = otherGO.transform.parent.GetComponent<Piece>();
            //if up piece not null and other piece is this pieces up piece and that the adapters match.
            if (upPiece != null &&
            otherPiece == upPiece &&
            otherPiece.GetBottomAdapter() == otherAdapter)
            {
                if(!m_potentialMatches.Contains(otherPiece)) m_potentialMatches.Add(otherPiece);
            }

            //if right piece not null and other piece is this pieces up piece and that the adapters match.
            if (rightPiece != null &&
            otherPiece == rightPiece &&
            otherPiece.GetLeftAdapter() == otherAdapter)
            {
                if (!m_potentialMatches.Contains(otherPiece)) m_potentialMatches.Add(otherPiece);
            }

            //if bottom piece not null and other piece is this pieces up piece and that the adapters match.
            if (downPiece != null &&
            otherPiece == downPiece &&
            otherPiece.GetTopAdapter() == otherAdapter)
            {
                if (!m_potentialMatches.Contains(otherPiece)) m_potentialMatches.Add(otherPiece);
            }

            //if left piece not null and other piece is this pieces up piece and that the adapters match.
            if (leftPiece != null &&
            otherPiece == leftPiece &&
            otherPiece.GetRightAdapter() == otherAdapter)
            {
                if (!m_potentialMatches.Contains(otherPiece)) m_potentialMatches.Add(otherPiece);
            }
        }
    }
    
    void SetChildrenRecursively(Transform t)
    {
        if (t == transform) return;
        t.SetParent(transform);
        foreach(Transform child in t.GetComponentInChildren<Transform>())
        {
            SetChildrenRecursively(child);
        }
    }

    public void Align(Transform t, Direction d)
    {
        if(!m_movedThisFrame)
        {
            Debug.Log("Align " + name);
            m_movedThisFrame = true;
            if (d == Direction.UP)
            {
                // align by column (x)
                this.transform.position = new Vector3(t.position.x, t.position.y + .666f, 0f);
            }
            if (d == Direction.RIGHT)
            {
                // align by row (y)
                this.transform.position = new Vector3(t.position.x + .666f, t.position.y, 0f);
            }
            if (d == Direction.DOWN)
            {
                // align by column (x)
                this.transform.position = new Vector3(t.position.x, t.position.y - .666f, 0f);
            }
            if (d == Direction.LEFT)
            {
                // align by row (y)
                this.transform.position = new Vector3(t.position.x - .666f, t.position.y, 0f);
            }
            StartAlignment();
        }
        
    }
    
    void OnMouseDown()
    {
        m_held = true;
    }
    
    public void AssignPieceAsSuccessfulMatch(Piece p)
    {
        if(!m_successfulMatches.Contains(p))
        {
            m_successfulMatches.Add(p);
        }
    }

    void OnMouseUp()
    {
        m_held = false;
        // handle attaching piece
        foreach (Piece p in m_potentialMatches)
        {
            if (p == upPiece)
            {
                Debug.Log("Snap my up piece");
                transform.position = new Vector3(p.transform.position.x, p.transform.position.y - .666f, 0f);
                if (!m_successfulMatches.Contains(p))
                {
                    m_successfulMatches.Add(p);
                    p.AssignPieceAsSuccessfulMatch(this);
                }
            }
            if (p == rightPiece)
            {
                Debug.Log("Snap my right piece");
                transform.position = new Vector3(p.transform.position.x - .666f, p.transform.position.y, 0f);
                if (!m_successfulMatches.Contains(p))
                {
                    m_successfulMatches.Add(p);
                    p.AssignPieceAsSuccessfulMatch(this);
                }
            }
            if (p == downPiece)
            {
                Debug.Log("Snap my down piece");
                transform.position = new Vector3(p.transform.position.x, p.transform.position.y + .666f, 0f);
                if (!m_successfulMatches.Contains(p))
                {
                    m_successfulMatches.Add(p);
                    p.AssignPieceAsSuccessfulMatch(this);
                }
            }
            if (p == leftPiece)
            {
                Debug.Log("Snap my left piece");
                transform.position = new Vector3(p.transform.position.x + .666f, p.transform.position.y, 0f);
                if (!m_successfulMatches.Contains(p))
                {
                    m_successfulMatches.Add(p);
                    p.AssignPieceAsSuccessfulMatch(this);
                }
            }
        }
        StartAlignment();
    }


    public void SetUpPiece(Piece p)
    {
        upPiece = p;
    }

    public void SetRightPiece(Piece p)
    {
        rightPiece = p;
    }

    public void SetDownPiece(Piece p)
    {
        downPiece = p;
    }

    public void SetLeftPiece(Piece p)
    {
        leftPiece = p;
    }
}
