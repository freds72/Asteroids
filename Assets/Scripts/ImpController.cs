using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class ImpController : MonoBehaviour {
    public GameObject Avatar;
    
    SpriteRenderer _renderer;
    Rigidbody _rb;

	// Use this for initialization
	void Start () {
        _renderer = Avatar.GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void LateUpdate () {
        _renderer.flipX = _rb.velocity.x < 0;
	}
}
