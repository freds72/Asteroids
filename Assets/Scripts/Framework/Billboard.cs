using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    public Camera Camera;
    public bool KeepLocalRotation = false;
    public bool KeepParentRotation = false;
    Transform _parent;

    void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;
    }

	// Use this for initialization
	void Start () {
        _parent = GetComponentInParent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate() {
        Quaternion localRotation = transform.localRotation;
        
        transform.LookAt(transform.position + Camera.transform.rotation * Vector3.forward,
               Camera.transform.rotation * Vector3.up);
        // reapply initial rotation
        if (KeepLocalRotation)
            transform.localRotation *= localRotation;
        if (KeepParentRotation)
        {
            transform.localRotation *= Quaternion.Euler(0, 0, _parent.localRotation.eulerAngles.y);
        }
	}
}
