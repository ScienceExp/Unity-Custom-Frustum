using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisibleFromCustomFrustum : MonoBehaviour {

    public CustomFrustumPlaneViewer customFrustumPlaneViewer;
    Renderer rend;

	void Start () {
        rend = this.GetComponent<Renderer>();
    }
	
	void Update () {
        if (rend.IsVisibleFrom(customFrustumPlaneViewer.frustum.planes))
            this.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
        else
            this.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
    }
}
