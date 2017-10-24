using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cropper : MonoBehaviour {


    private Camera camera;
    Vector3 offset;
    RectTransform rect;
    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rect = gameObject.GetComponent<RectTransform>();
    }

    public Texture2D GetCroppedImage()
    {
        Vector3 imgPos = CameraMenu.instance.imageObject.transform.position;
        Vector3 cropperPos = CameraMenu.instance.cropper.transform.position;

        Vector3 posDiff = imgPos + cropperPos;

        MeshCollider imgMC = CameraMenu.instance.imageObject.GetComponent<MeshCollider>();
        MeshCollider cropperMC = CameraMenu.instance.cropper.GetComponent<MeshCollider>();

        float imgXScale = CameraMenu.instance.imageObject.transform.localScale.x;
        float cropperXScale = CameraMenu.instance.cropper.transform.localScale.x;

        float imgYScale = CameraMenu.instance.imageObject.transform.localScale.y;
        float cropperYScale = CameraMenu.instance.cropper.transform.localScale.y;

        Texture2D img = CameraMenu.instance.imageObject.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        /*
        Debug.Log("img size:: " + img.width + "  " + img.height);

        Debug.Log("Img POs:: " + imgPos);
        Debug.Log("cropperPos POs:: " + cropperPos);

        Debug.Log("imgMC :: " + imgMC.bounds);
        Debug.Log("cropperMC :: " + cropperMC.bounds.center + "  " + cropperMC.transform.localScale);

        Debug.Log(posDiff);
        */
        float ratioX = imgXScale / cropperXScale;
        float ratioY = imgYScale / cropperYScale;

        float xsize = img.width / ratioX;
        float ysize = img.height / ratioY;

        //Debug.Log("x - y :: " + xsize + " " + ysize);

        float startPosX = imgXScale - (imgXScale / 2f - cropperPos.x + cropperXScale / 2f);
        float startPosY = (imgYScale / 2f + cropperPos.y - cropperYScale / 2f);

        Debug.Log("startPosX -. startPosY :: " + startPosX + " " + startPosY);

        startPosX = 1 / (imgXScale / startPosX) * img.width;
        startPosY = 1 / (imgYScale / startPosY) * img.height;

        //Debug.Log("startPosX -. startPosY :: " + startPosX + " " + startPosY);

        int size = (int)ysize;
        int x = (int)startPosX;
        int y = (int)startPosY;
        Texture2D newImg = CropTexture(img, x, y, size);
        Material m = new Material(Shader.Find("Custom/Puzzle Piece"));
        CameraMenu.instance.texture = newImg;
        
        GameObject.Find("Quad").GetComponent<MeshRenderer>().material = m;
        GameObject.Find("Quad").GetComponent<MeshRenderer>().material.mainTexture = newImg;
        return newImg;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
        }

    }

    Texture2D CropTexture(Texture2D original, int x, int y, int size)
    {
        Texture2D cropped = new Texture2D(size, size);
        int original_y = y;

        int xN = x + size;
        int yN = y + size;

        int i = 0;
        int j = 0;

        for (; x < xN; x++)
        {
            for (y = original_y; y < yN; y++)
            {
                cropped.SetPixel(i, j, original.GetPixel(x,y));
                j++;
                //Debug.Log("i,j" + i + " " + j);
            }
            y = original_y;
            j = 0;
            i++;
        }
        cropped.Apply();

        return cropped;
    }

}
