using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class F35 : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;

	// Use this for initialization
	void Start () {
        List<Vector3> points = new List<Vector3>(new Vector3[] { new Vector3(-0.31f, -0.127f, 0f), new Vector3(-0.31f, -0.24f, 0f), new Vector3(-0.117f, -0.015f, 0f), new Vector3(-0.31f, -0.127f, 0f), new Vector3(-0.099f, 0.171f, 0f), new Vector3(-0.117f, -0.015f, 0f), new Vector3(-0.099f, 0.171f, 0f), new Vector3(-0.051f, 0.215f, 0f), new Vector3(-0.027f, 0.361f, 0f), new Vector3(-0.051f, 0.215f, 0f), new Vector3(0f, 0.4f, 0f), new Vector3(-0.027f, 0.361f, 0f), new Vector3(0.027f, 0.361f, 0f), new Vector3(0f, 0.4f, 0f), new Vector3(0.027f, 0.361f, 0f), new Vector3(0.051f, 0.215f, 0f), new Vector3(0.099f, 0.171f, 0f), new Vector3(0.051f, 0.215f, 0f), new Vector3(0.117f, -0.015f, 0f), new Vector3(0.099f, 0.171f, 0f), new Vector3(0.117f, -0.015f, 0f), new Vector3(0.31f, -0.127f, 0f), new Vector3(0.31f, -0.24f, 0f), new Vector3(0.31f, -0.127f, 0f), new Vector3(0.106f, -0.285f, 0f), new Vector3(0.31f, -0.24f, 0f), new Vector3(0.106f, -0.285f, 0f), new Vector3(0.224f, -0.372f, 0f), new Vector3(0.225f, -0.423f, 0f), new Vector3(0.224f, -0.372f, 0f), new Vector3(0.225f, -0.423f, 0f), new Vector3(0.059f, -0.459f, 0f), new Vector3(0.041f, -0.347f, 0f), new Vector3(0.059f, -0.459f, 0f), new Vector3(0.041f, -0.347f, 0f), new Vector3(-0.041f, -0.347f, 0f), new Vector3(-0.059f, -0.459f, 0f), new Vector3(-0.041f, -0.347f, 0f), new Vector3(-0.059f, -0.459f, 0f), new Vector3(-0.225f, -0.423f, 0f), new Vector3(-0.225f, -0.423f, 0f), new Vector3(-0.224f, -0.372f, 0f), new Vector3(-0.224f, -0.372f, 0f), new Vector3(-0.106f, -0.285f, 0f), new Vector3(-0.106f, -0.285f, 0f), new Vector3(-0.31f, -0.24f, 0f) });
        VectorLine line;
        if ( LineTexture == null )
            line = new VectorLine(GetType().Name, points, LineWidth);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
