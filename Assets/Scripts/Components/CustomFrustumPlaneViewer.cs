using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFrustumPlaneViewer : MonoBehaviour
{
    public Camera cam;
    [Header("Plane Settings")]
    public float FOV = 60f;
    public float nearClipPlane = 0f;
    public float farClipPlane = 400f;
    [Tooltip("Should be set as low as the lowest vertex in the level")]
    public float bottomPlane = -400f;
    [Tooltip("Should be set as high as the highest vertex in the level")]
    public float topPlane = 400f;
    public bool showDebug = true;
    public CustomFrustumPlane frustum;

    void Start()
    {
        frustum = new CustomFrustumPlane(cam, FOV, farClipPlane, topPlane, bottomPlane);
    }

    void Update()
    {
        frustum.UpdatePlanes(showDebug);
    }
}
