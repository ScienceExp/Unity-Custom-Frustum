using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFrustumPlane
{
    enum planeDir
    {
        //Ordering: [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far
        left,
        right,
        bottom,
        top,
        near,
        far
    }

    Camera cam;
    /// <summary>should be wider than camera FOV so preloading objects on sides are not seen</summary>
    public float FOV = 90f;
    /// <summary>should be 0</summary>
    public float nearClipPlane = 0f;
    /// <summary>"Should be set a little farther than camera clip plane for loading objects so you do not see popping"</summary>
    public float farClipPlane = 400f;
    /// <summary>"Should be set as low as the lowest vertex in the level"</summary>
    public float bottomPlane = -400f;
    /// <summary>"Should be set as high as the highest vertex in the level"</summary>
    public float topPlane = 400f;
    /// <summary>Used to add 1/2 fov angle to each side of forward vector</summary>
    float halfFov;
    /// <summary>Custom FOV planes that can be used for real time loading/deleting of objects</summary>
    public Plane[] planes;

    public CustomFrustumPlane(Camera c, float fov, float farPlaneDist = 400f, float topPlaneDist = 400f, float bottomPlaneDist = -400f)
    {
        cam = c;
        planes = new Plane[6];
        FOV = fov;
        farClipPlane = farPlaneDist;
        topPlane = topPlaneDist;
        bottomPlane = bottomPlaneDist;
        halfFov = FOV / 2f;
    }

    public void UpdatePlanes(bool showDebug = false)
    {

        float hypotenus = farClipPlane / Mathf.Cos(Mathf.Deg2Rad * halfFov);

        #region Far
        Vector3 angleF = new Vector3(0, cam.transform.eulerAngles.y, 0);
        Vector3 vF = Quaternion.Euler(angleF) * Vector3.forward * farClipPlane;
        Vector3 cF = -vF.normalized;
        planes[(int)planeDir.far] = new Plane(cF, cam.transform.position + vF);
        #endregion

        #region Far
        //Vector3 angleN = new Vector3(0, cam.transform.eulerAngles.y, 0);
        //Vector3 vF = Quaternion.Euler(angleF) * Vector3.forward * farClipPlane;
        Vector3 cN = vF.normalized;
        planes[(int)planeDir.near] = new Plane(cN, cam.transform.position);
        #endregion

        #region Left
        Vector3 angleL = new Vector3(0, cam.transform.eulerAngles.y - halfFov, 0);
        Vector3 vL = Quaternion.Euler(angleL) * Vector3.forward * hypotenus;
        Vector3 vL2 = new Vector3(vL.x, vL.y + 10, vL.z);
        Vector3 cL = Vector3.Cross(vL2, vL).normalized;
        planes[(int)planeDir.left] = new Plane(cL, cam.transform.position);
        #endregion

        #region Right
        Vector3 angleR = new Vector3(0, cam.transform.eulerAngles.y + halfFov, 0);  //get camera angle + halfFOV angle
        Vector3 vR = Quaternion.Euler(angleR) * Vector3.forward * hypotenus;        //get vector projected out at angle
        Vector3 vR2 = new Vector3(vR.x, vR.y + 10, vR.z);                           //get vector of plane to use with cross product
        Vector3 cR = Vector3.Cross(vR, vR2).normalized;                             //get normal vector for plane
        planes[(int)planeDir.right] = new Plane(cR, cam.transform.position);        //need normal and intersection point
        #endregion

        #region Forward and Back planes
        planes[(int)planeDir.bottom] = new Plane(Vector3.up, cam.transform.position + new Vector3(0f, bottomPlane, 0f));
        planes[(int)planeDir.top] = new Plane(Vector3.down, cam.transform.position + new Vector3(0f, topPlane, 0f));
        #endregion

        if (showDebug)
        {
            float dist = farClipPlane;
            //red is the bounds
            Debug.DrawLine(cam.transform.position, cam.transform.position + vL, Color.red);
            Debug.DrawLine(cam.transform.position, cam.transform.position + vR, Color.red);
            Debug.DrawLine(cam.transform.position + vL, cam.transform.position + vR, Color.red);
            //Blue is the normals
            Debug.DrawLine(cam.transform.position, cam.transform.position + cR * dist, Color.blue);
            Debug.DrawLine(cam.transform.position, cam.transform.position + cL * dist, Color.blue);
            Debug.DrawLine(cam.transform.position + vF, cam.transform.position + vF + cF * dist, Color.blue);
            Debug.DrawLine(cam.transform.position, cam.transform.position + cN * dist, Color.blue);
        }
    }
}
