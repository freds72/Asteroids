using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class SU27 : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;

	// Use this for initialization
	void Start () {	
        List<Vector3> points = new List<Vector3>(new Vector3[] { new Vector3(-0.183f, -0.478f, 0f), new Vector3(-0.212f, -0.434f, 0f), new Vector3(-0.212f, -0.434f, 0f), new Vector3(-0.09f, -0.315f, 0f), new Vector3(-0.089f, -0.464f, 0f), new Vector3(-0.183f, -0.478f, 0f), new Vector3(-0.09f, -0.315f, 0f), new Vector3(-0.089f, -0.281f, 0f), new Vector3(-0.089f, -0.281f, 0f), new Vector3(-0.318f, -0.342f, 0f), new Vector3(-0.318f, -0.342f, 0f), new Vector3(-0.319f, -0.265f, 0f), new Vector3(-0.319f, -0.265f, 0f), new Vector3(-0.087f, -0.047f, 0f), new Vector3(-0.076f, -0.414f, 0f), new Vector3(-0.089f, -0.464f, 0f), new Vector3(-0.087f, -0.047f, 0f), new Vector3(-0.086f, -0.038f, 0f), new Vector3(-0.086f, -0.038f, 0f), new Vector3(-0.06f, 0.029f, 0f), new Vector3(-0.068f, -0.466f, 0f), new Vector3(-0.076f, -0.414f, 0f), new Vector3(-0.031f, -0.467f, 0f), new Vector3(-0.068f, -0.466f, 0f), new Vector3(-0.06f, 0.029f, 0f), new Vector3(-0.03f, 0.223f, 0f), new Vector3(-0.022f, -0.424f, 0f), new Vector3(-0.031f, -0.467f, 0f), new Vector3(-0.03f, 0.223f, 0f), new Vector3(-0.027f, 0.28f, 0f), new Vector3(-0.027f, 0.28f, 0f), new Vector3(-0.016f, 0.358f, 0f), new Vector3(0f, -0.558f, 0f), new Vector3(-0.022f, -0.424f, 0f), new Vector3(-0.016f, 0.358f, 0f), new Vector3(0f, 0.4f, 0f), new Vector3(0.022f, -0.424f, 0f), new Vector3(0f, -0.558f, 0f), new Vector3(0f, 0.4f, 0f), new Vector3(0.016f, 0.358f, 0f), new Vector3(0.016f, 0.358f, 0f), new Vector3(0.027f, 0.28f, 0f), new Vector3(0.031f, -0.467f, 0f), new Vector3(0.022f, -0.424f, 0f), new Vector3(0.027f, 0.28f, 0f), new Vector3(0.03f, 0.223f, 0f), new Vector3(0.03f, 0.223f, 0f), new Vector3(0.06f, 0.029f, 0f), new Vector3(0.068f, -0.466f, 0f), new Vector3(0.031f, -0.467f, 0f), new Vector3(0.06f, 0.029f, 0f), new Vector3(0.086f, -0.038f, 0f), new Vector3(0.076f, -0.414f, 0f), new Vector3(0.068f, -0.466f, 0f), new Vector3(0.089f, -0.464f, 0f), new Vector3(0.076f, -0.414f, 0f), new Vector3(0.086f, -0.038f, 0f), new Vector3(0.087f, -0.047f, 0f), new Vector3(0.089f, -0.281f, 0f), new Vector3(0.09f, -0.315f, 0f), new Vector3(0.183f, -0.478f, 0f), new Vector3(0.089f, -0.464f, 0f), new Vector3(0.09f, -0.315f, 0f), new Vector3(0.212f, -0.434f, 0f), new Vector3(0.212f, -0.434f, 0f), new Vector3(0.183f, -0.478f, 0f), new Vector3(0.087f, -0.047f, 0f), new Vector3(0.319f, -0.265f, 0f), new Vector3(0.319f, -0.265f, 0f), new Vector3(0.318f, -0.342f, 0f), new Vector3(0.318f, -0.342f, 0f), new Vector3(0.089f, -0.281f, 0f)});
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
