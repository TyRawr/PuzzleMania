using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject piecePrefab;

    [SerializeField]
    private Texture2D connector, invertedConnector, wall;
    private Texture2D rightConnector, rightInverted, rightWall;
    private Texture2D downConnector, downInverted, downWall;
    private Texture2D leftConnector, leftInverted, leftWall;
    private Camera camera;

    [SerializeField]
    private int cols, rows;

    private Renderer[,] pieceRenderers;
    public string url = "https://s3-us-west-2.amazonaws.com/puzzle-tyrawr/images/giraffe.jpeg";
    public string assetName;

    IEnumerator GetImage(string imgName = "duck")
    {
        AssetBundle myLoadedAssetBundle = null;
        AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
        Debug.Log("number of bundles " + bundles.Length);

        for (int i = 0; i < bundles.Length; i++)
        {
            Debug.Log("Bundle: " + bundles[i].name);
            if(bundles[i].name == "images.unity3d")
            {
                myLoadedAssetBundle = bundles[i];
            }
        }
        if(myLoadedAssetBundle == null)
        {
            while (!Caching.ready)
                yield return null;
            Caching.CleanCache();
            var www = WWW.LoadFromCacheOrDownload("https://s3-us-west-2.amazonaws.com/puzzle-tyrawr/images/images.unity3d", 5);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield return null;
            }
            myLoadedAssetBundle = www.assetBundle;
        }
        Texture2D tex = myLoadedAssetBundle.LoadAsset("Assets/Images/" + imgName + ".jpeg") as Texture2D;
        GameObject piece = GameObject.Find("Piece");
        piece.GetComponent<Renderer>().material.mainTexture = tex;
        BuildPieces();
    }
    void Start()
    {
        camera = Camera.main;
        pieceRenderers = new Renderer[rows, cols];
        rightConnector = RotateTexture(connector, true);
        rightInverted = RotateTexture(invertedConnector, true);
        rightWall = RotateTexture(wall, true);

        leftConnector = RotateTexture(connector, false);
        leftInverted = RotateTexture(invertedConnector, false);
        leftWall = RotateTexture(wall, false);

        downConnector = FlipTexture(connector);
        downInverted = FlipTexture(invertedConnector);
        downWall = FlipTexture(wall);
        StartCoroutine(GetImage());
    }

    bool multiTouch = false;
    float dist = 2f;
    float dist1 = 2f;
    Vector3 p1 = new Vector3(0f, 0f, 0f);
    Vector3 p2;
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.


    private void Update()
    {
        Debug.Log((float)Screen.width);
        Debug.Log((float)Screen.height);
        Debug.Log((float)Screen.width / (float)Screen.height);

        if(Input.touchCount >= 2)
        {
            //STOP HERE

            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            if(!multiTouch)
            {
                multiTouch = true;
                dist = Vector3.Distance(touchZero.position, touchOne.position);
                dist1 = dist;
            } else
            {
                dist = dist1;
                dist1 = Vector3.Distance(touchZero.position, touchOne.position);
            }

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition; 

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            // ... change the orthographic size based on the change in distance between the touches.
            camera.orthographicSize += (dist - dist1)/100f;
            if(camera.orthographicSize < 1)
            {
                camera.orthographicSize = 1;
            }
            if(camera.orthographicSize > 8)
            {
                camera.orthographicSize = 8;
            }

            // Make sure the orthographic size never drops below zero.
            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            
        } else
        {
            multiTouch = false;
        }
       
    }

    Texture2D FlipTexture(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);

        int xN = original.width;
        int yN = original.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(i, yN - j - 1, original.GetPixel(i, j));
            }
        }
        flipped.Apply();

        return flipped;
    }

    Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    Texture2D RotateTextureLeft(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);

        int xN = original.width;
        int yN = original.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(xN - i - 1, yN - j - 1, original.GetPixel(i, j));
            }
        }
        flipped.Apply();

        return flipped;
    }

    public enum EDGETYPE {
        WALL,
        CONNECTOR,
        INVERSE
    } 

    Texture2D GetMaskFromEdgeAndConnectionType(EDGETYPE type, string direction)
    {
        Texture2D texture = wall;
        if(direction == "up")
        {
            if(type == EDGETYPE.WALL)
            {
                texture = wall;
            } 
            if(type == EDGETYPE.INVERSE)
            {
                texture = invertedConnector;
            }
            if (type == EDGETYPE.CONNECTOR)
            {
                texture = connector;
            }
        }
        
        if(direction == "right")
        {
            if (type == EDGETYPE.WALL)
            {
                texture = rightWall;
            }
            if (type == EDGETYPE.INVERSE)
            {
                texture = rightInverted;
            }
            if (type == EDGETYPE.CONNECTOR)
            {
                texture = rightConnector;
            }
        }

        if (direction == "down")
        {
            if (type == EDGETYPE.WALL)
            {
                texture = downWall;
            }
            if (type == EDGETYPE.INVERSE)
            {
                texture = downInverted;
            }
            if (type == EDGETYPE.CONNECTOR)
            {
                texture = downConnector;
            }
        }

        if (direction == "left")
        {
            if (type == EDGETYPE.WALL)
            {
                texture = leftWall;
            }
            if (type == EDGETYPE.INVERSE)
            {
                texture = leftInverted;
            }
            if (type == EDGETYPE.CONNECTOR)
            {
                texture = leftConnector;
            }
        }
        return texture;
    }

    void BuildPieces()
    {
        Object[] oldPieces = GameObject.FindObjectsOfType(typeof(Piece));
        foreach(Piece p in oldPieces)
        {
            if(p.name != "Piece")
            {
                GameObject.Destroy(p.gameObject);
            }
        }

        Vector3 offset = Vector3.zero;
        offset.x = -(float)cols / 2.0f;
        offset.y = -(float)rows / 2.0f;
        float startX = offset.x;
        float uvWidth = 1.0f / cols;
        float uvHeight = 1.0f / rows;

        Piece[,] pieces = new Piece[rows, cols];
        GameObject gameboard = GameObject.Find("Gameboard");
        for (int j = 0; j < cols; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                Piece piece = ((GameObject)Instantiate(piecePrefab)).GetComponent<Piece>();
                pieces[j, i] = piece;
                piece.transform.position = offset;
                piece.transform.localScale = Vector3.one * 2f;
                pieceRenderers[j, i] = piece.GetComponent<Renderer>();
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uvs = mesh.uv;
                mesh.uv2 = uvs;
                uvs[0] = new Vector2((i - 1) * uvWidth, (j - 1) * uvHeight);
                uvs[3] = new Vector2((i - 1) * uvWidth, (j + 2) * uvHeight);
                uvs[1] = new Vector2((i + 2) * uvWidth, (j + 2) * uvHeight);
                uvs[2] = new Vector2((i + 2) * uvWidth, (j - 1) * uvHeight);
                mesh.uv = uvs;

                offset.x += 1.25f;
                piece.transform.SetParent(gameboard.transform);
            }
            offset.y += 1.25f;
            offset.x = startX;
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Piece piece = pieces[row, col];
                EDGETYPE up, right, down, left;

                if (row == 0)
                {
                    //randomly pick a connection style
                    down = EDGETYPE.WALL;
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        up = EDGETYPE.INVERSE;
                    }
                    else
                    {
                        up = EDGETYPE.CONNECTOR;
                    }
                }
                else
                {
                    // grab piece below and set matching
                    Piece below = pieces[row - 1, col];
                    piece.SetDownPiece(below);
                    below.SetUpPiece(piece);
                    if (below.up == EDGETYPE.CONNECTOR)
                    {
                        down = EDGETYPE.INVERSE;
                    }
                    else
                    {
                        down = EDGETYPE.CONNECTOR;
                    }
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        up = EDGETYPE.INVERSE;
                    }
                    else
                    {
                        up = EDGETYPE.CONNECTOR;
                    }
                }

                if (col == 0)
                {
                    left = EDGETYPE.WALL;
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        right = EDGETYPE.INVERSE;
                    }
                    else
                    {
                        right = EDGETYPE.CONNECTOR;
                    }
                }
                else
                {
                    // grab piece left and set matching
                    Piece leftPiece = pieces[row, col - 1];
                    piece.SetLeftPiece(leftPiece);
                    leftPiece.SetRightPiece(piece);
                    if (leftPiece.right == EDGETYPE.CONNECTOR)
                    {
                        left = EDGETYPE.INVERSE;
                    }
                    else
                    {
                        left = EDGETYPE.CONNECTOR;
                    }
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        right = EDGETYPE.INVERSE;
                    }
                    else
                    {
                        right = EDGETYPE.CONNECTOR;
                    }
                }

                if (row == rows - 1)
                {
                    up = EDGETYPE.WALL;
                }
                if (col == cols - 1)
                {
                    right = EDGETYPE.WALL;
                }
                piece.name = "Piece " + row + " " + col;
                pieces[row, col].up = up;
                pieces[row, col].right = right;
                pieces[row, col].down = down;
                pieces[row, col].left = left;
                pieces[row, col].row = row;
                pieces[row, col].col = col;
                Texture2D upMask, rightMask, downMask, leftMask;
                //get actual textures.
                upMask = GetMaskFromEdgeAndConnectionType(up, "up");
                rightMask = GetMaskFromEdgeAndConnectionType(right, "right");
                downMask = GetMaskFromEdgeAndConnectionType(down, "down");
                leftMask = GetMaskFromEdgeAndConnectionType(left, "left");

                Renderer renderer = pieceRenderers[row, col];
                MaterialPropertyBlock props = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(props);
                props.SetColor("_Color", Color.white);
                props.SetTexture("_Mask", upMask);
                props.SetTexture("_Mask2", rightMask);
                props.SetTexture("_Mask3", downMask);
                props.SetTexture("_Mask4", leftMask);
                renderer.SetPropertyBlock(props);
            }
        }
    }

    private void OnGUI()
    {
        /*
        if (GUI.Button(new Rect(10, 10, 300 , 150), "Reset Puzzle"))
        {
            BuildPieces();
            //AndroidDialog dialog = AndroidDialog.Create("Test", "Message");
        }
        */
        if (GUI.Button(new Rect(10, 10, 300, 150), "Load Duck"))
        {
            StartCoroutine(GetImage());
        }
        if (GUI.Button(new Rect(10, 160, 300, 150), "Load Giraffe"))
        {
            StartCoroutine(GetImage("giraffe"));
        }

    }
}

public enum Direction
{
    UP, RIGHT, DOWN, LEFT
}