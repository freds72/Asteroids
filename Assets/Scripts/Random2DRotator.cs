using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Random2DRotator : MonoBehaviour {

    public float RotationScale = 25;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = RotationScale * Random.Range(-1,1);
    }
}
