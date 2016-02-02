using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

    public float RotationScale = 25;

    void Start()
    {
        float force = Random.Range(0, RotationScale);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if ( rb != null )
            rb.angularVelocity = force * Random.Range(-1, 1);
        else
        {
            Rigidbody rb3d = GetComponent<Rigidbody>();
            rb3d.angularVelocity = force * Random.onUnitSphere;
        }
    }
}
