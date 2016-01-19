using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class SU27 : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;

	// Use this for initialization
	void Start () {
        List<Vector3> points = new List<Vector3>(new Vector3[] { new Vector3(-0.183f, -0.311f, 0f), new Vector3(-0.212f, -0.267f, 0f), new Vector3(-0.212f, -0.267f, 0f), new Vector3(-0.09f, -0.148f, 0f), new Vector3(-0.089f, -0.297f, 0f), new Vector3(-0.183f, -0.311f, 0f), new Vector3(-0.09f, -0.148f, 0f), new Vector3(-0.089f, -0.114f, 0f), new Vector3(-0.089f, -0.114f, 0f), new Vector3(-0.318f, -0.176f, 0f), new Vector3(-0.318f, -0.176f, 0f), new Vector3(-0.319f, -0.098f, 0f), new Vector3(-0.319f, -0.098f, 0f), new Vector3(-0.087f, 0.119f, 0f), new Vector3(-0.08f, -0.265f, 0f), new Vector3(-0.089f, -0.297f, 0f), new Vector3(-0.087f, 0.119f, 0f), new Vector3(-0.086f, 0.129f, 0f), new Vector3(-0.086f, 0.129f, 0f), new Vector3(-0.06f, 0.195f, 0f), new Vector3(-0.021f, -0.264f, 0f), new Vector3(-0.08f, -0.265f, 0f), new Vector3(-0.06f, 0.195f, 0f), new Vector3(-0.03f, 0.39f, 0f), new Vector3(-0.03f, 0.39f, 0f), new Vector3(-0.027f, 0.447f, 0f), new Vector3(-0.027f, 0.447f, 0f), new Vector3(-0.016f, 0.524f, 0f), new Vector3(0f, -0.391f, 0f), new Vector3(-0.021f, -0.264f, 0f), new Vector3(-0.016f, 0.524f, 0f), new Vector3(0f, 0.567f, 0f), new Vector3(0.021f, -0.264f, 0f), new Vector3(0f, -0.391f, 0f), new Vector3(0f, 0.567f, 0f), new Vector3(0.016f, 0.524f, 0f), new Vector3(0.016f, 0.524f, 0f), new Vector3(0.027f, 0.447f, 0f), new Vector3(0.027f, 0.447f, 0f), new Vector3(0.03f, 0.39f, 0f), new Vector3(0.08f, -0.265f, 0f), new Vector3(0.021f, -0.264f, 0f), new Vector3(0.03f, 0.39f, 0f), new Vector3(0.06f, 0.195f, 0f), new Vector3(0.06f, 0.195f, 0f), new Vector3(0.086f, 0.129f, 0f), new Vector3(0.089f, -0.297f, 0f), new Vector3(0.08f, -0.265f, 0f), new Vector3(0.086f, 0.129f, 0f), new Vector3(0.087f, 0.119f, 0f), new Vector3(0.089f, -0.114f, 0f), new Vector3(0.089f, -0.297f, 0f), new Vector3(0.089f, -0.114f, 0f), new Vector3(0.09f, -0.148f, 0f), new Vector3(0.09f, -0.148f, 0f), new Vector3(0.089f, -0.297f, 0f), new Vector3(0.183f, -0.311f, 0f), new Vector3(0.089f, -0.297f, 0f), new Vector3(0.09f, -0.148f, 0f), new Vector3(0.212f, -0.267f, 0f), new Vector3(0.212f, -0.267f, 0f), new Vector3(0.183f, -0.311f, 0f), new Vector3(0.087f, 0.119f, 0f), new Vector3(0.319f, -0.098f, 0f), new Vector3(0.319f, -0.098f, 0f), new Vector3(0.318f, -0.176f, 0f), new Vector3(0.318f, -0.176f, 0f), new Vector3(0.089f, -0.114f, 0f) });
        VectorLine line;
        if ( LineTexture == null )
            line = new VectorLine("SU27", points, LineWidth);
        else
            line = new VectorLine("SU27", points, LineTexture, LineWidth);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
