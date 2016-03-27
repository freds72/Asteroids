using UnityEngine;
using System.Collections;

public class BeholderController : MonoBehaviour {

    public float RotationSpeed = 1;
    public GameObject RotationTarget;
    Stationaries _stations;

	// Use this for initialization
	void Start () {
        _stations = GetComponent<Stationaries>();
	}
	
	// Update is called once per frame
	void Update () {
        RotationTarget.transform.rotation = Quaternion.Euler(0, RotationSpeed * Time.time, 0);

        _stations.Release();
	}
}
