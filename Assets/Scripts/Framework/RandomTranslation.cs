using UnityEngine;
using System.Collections;

public class RandomTranslation : MonoBehaviour {
    /// <summary>
    /// Velocity
    /// </summary>
    public float MinVelocity = 10;
    public float MaxVelocity = 12;
    public bool Is3D = false;

    float _velocity = 0;
    // Use this for initialization
    void Start()
    {
        _velocity = Random.Range(MinVelocity, MaxVelocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Time.deltaTime * _velocity * (Is3D ? transform.right : transform.up);
    }
}
