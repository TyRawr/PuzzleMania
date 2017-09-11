using UnityEngine;
using System.Collections;

public class RotateUVs : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    protected void Update()
    {
        // Construct a rotation matrix and set it for the shader
        Matrix4x4 t = Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one);

        Quaternion rotation = Quaternion.Euler(0, 0, Time.time * rotateSpeed);
        Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

        Matrix4x4 tInv = Matrix4x4.TRS(pivot, Quaternion.identity, Vector3.one);
        GetComponent<Renderer>().material.SetMatrix("_Rotation", tInv * r * t);
    }
}