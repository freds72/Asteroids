using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class FlightPlan : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float LineTextureScale = 1.0f;

    // Use this for initialization
	void Start () {
	    // list all Waypoint child objects
        int i = 1;
        List<Vector3> points = new List<Vector3>();
        foreach(Waypoint it in GetComponentsInChildren<Waypoint>())
        {
            it.ID = i++;
            points.Add(it.transform.position);
        }

        // draw a line between all waypoints
        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine(GetType().Name, points, LineWidth, LineType.Continuous, Joins.None);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Continuous, Joins.None);
        line.textureScale = LineTextureScale;

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
