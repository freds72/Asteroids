using UnityEngine;
using System.Collections;

/// <summary>
/// Simple top-down "plane" controller
/// </summary>
public class PlaneController : MonoBehaviour {

    public GameObject RotationTarget;
    public float RotationSpeed = 1.0f;
    public float Velocity = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        float lh = Input.GetAxis("LeftStickHorizontal");

        Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
        t.rotation *= Quaternion.Euler(0.0f, 0.0f, -lh * RotationSpeed * Time.deltaTime);

        this.transform.position += Time.deltaTime * Velocity * t.up;
    }
}
