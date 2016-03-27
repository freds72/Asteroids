using UnityEngine;
using System.Collections;

/// <summary>
/// Moves an object along the Vector3.up direction.
/// </summary>
public class ConstantTranslation : MonoBehaviour {
    /// <summary>
    /// Velocity
    /// </summary>
    public float Velocity = 10;
    public bool Is3D = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position += Time.deltaTime * Velocity * (Is3D?transform.right:transform.up);
	}
}
