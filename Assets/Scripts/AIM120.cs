using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class AIM120 : MonoBehaviour {
    public float Velocity = 10;
    public float BlastRadius = 0.5f; // kms
    public float Damage = 10;
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float Size = 1;

	// Use this for initialization
	void Start () {
        float hsize = Size / 2;
        List<Vector3> points = new List<Vector3>(new Vector3[] {
            new Vector3(0,hsize,0),
            new Vector3(0,-hsize,0),
            new Vector3(-0.25f * hsize,0.7f * hsize,0),
            new Vector3(0.25f * hsize,0.7f * hsize,0),
            new Vector3(-0.25f * hsize,-0.8f * hsize,0),
            new Vector3(0.25f * hsize,-0.8f * hsize,0),
        });
        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine(GetType().Name, points, LineWidth, LineType.Discrete, Joins.None);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Discrete, Joins.None);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * Velocity * transform.up;

    }
}
