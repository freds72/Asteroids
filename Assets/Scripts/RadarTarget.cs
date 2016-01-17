using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class RadarTarget : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float TextureScale = 1;
    public float Size = 1;

	// Use this for initialization
	void Start () {
        float radius = Size / 2;

        List<Vector3> points = new List<Vector3>(new Vector3[] { 
            new Vector3(0, radius, 0),
            new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * 210), radius * Mathf.Sin(Mathf.Deg2Rad * 210), 0),
            new Vector3(radius * Mathf.Cos(-Mathf.Deg2Rad * 30), radius * Mathf.Sin(-Mathf.Deg2Rad * 30), 0),
            new Vector3(0, radius, 0),
        });

        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine("RadarTarget", points, LineWidth, LineType.Continuous, Joins.Weld);
        else
            line = new VectorLine("RadarTarget", points, LineTexture, LineWidth, LineType.Continuous, Joins.Weld);
        line.textureScale = TextureScale;
        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
