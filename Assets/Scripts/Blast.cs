using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Blast : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float LineTextureScale = 1;
    public int Precision = 8;
    public float Radius = 1;

	// Use this for initialization
	void Start () {
        VectorLine blast = new VectorLine(GetType().Name, new List<Vector3>(Precision), LineTexture, LineWidth, LineType.Continuous, Joins.Weld);
        blast.MakeCircle(Vector3.zero, Radius);

        VectorManager.ObjectSetup(gameObject, blast, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
