using UnityEngine;
using System.Collections;

public class SU27Controller : MonoBehaviour {
    public float Velocity = 0.18f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * Velocity * transform.up;
	}
}
