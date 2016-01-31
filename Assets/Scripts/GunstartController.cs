using UnityEngine;
using System.Collections;

public class GunstartController : MonoBehaviour {
    public GameObject RotationTarget;
    public float RotationSpeed = 1.0f;
    public float Velocity = 1.0f;
    AudioSource _audio;

	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        float lh = Input.GetAxis("LeftStickHorizontal");
        float lv = Input.GetAxis("LeftStickVertical");

        Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
        t.rotation *= Quaternion.Euler(0.0f, 0.0f, -lh * RotationSpeed * Time.deltaTime);

        this.transform.position += (Time.deltaTime * lv * Velocity) * t.up;
	}
}
