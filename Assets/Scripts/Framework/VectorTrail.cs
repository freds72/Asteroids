using UnityEngine;
using System.Collections;
using Vectrosity;
using System.Collections.Generic;

public class VectorTrail : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float TrackingDelay = 0.1f;

    public Transform Target;
	// Use this for initialization
	void Start () {
        StartCoroutine(TrackTarget());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator TrackTarget()
    {
        List<Vector3> points = new List<Vector3>(new Vector3[] {           
            });

        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine(GetType().Name, points, LineWidth, LineType.Discrete, Joins.None);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Discrete, Joins.None);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);

        while (Target != null)
        {
            line.points3.Add(Target.transform.position);
            yield return new WaitForSeconds(TrackingDelay);
        }
    }
}
