using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
    public Vector3 Reference = Vector3.zero;
    public Transform Target;
    public float Scale = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 diff = Scale * (Target.transform.position - transform.position);

        transform.position = diff;
	}
}
