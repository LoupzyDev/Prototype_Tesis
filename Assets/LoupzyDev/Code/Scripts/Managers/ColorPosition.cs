using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ColorPosition : MonoBehaviour
{
    public float radius = 1f;
    public Vector3 scale;

    void Update()
    {
        Debug.Log("Escala Actual " + transform.localScale);
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
        Shader.SetGlobalVector("_Scale", scale);
        Shader.SetGlobalVector("_Rotation", transform.eulerAngles);

    }
}
