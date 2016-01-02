using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    float rotation = 0;
    Vector3 velocity = Vector3.zero;

    public float RotationSpeed = 25;
    public float MaxVelocity = 0.01f;

    public float MaxBulletVelocity = 10;
    public Rigidbody BulletPrefab;
    public float BulletDelay = 0.250f;
    public float BurstDelay = 0.750f;
    float _nextFire = 0;

    void Fire()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(BulletPrefab, transform.position, transform.rotation);
        bulletClone.velocity = transform.forward * MaxBulletVelocity;
        _nextFire = Time.time + BulletDelay;
    }

	// Update is called once per frame
	void Update () {
        float value = Input.GetAxisRaw("Rotation");
        if (Mathf.Abs(value) == 1)
            rotation += RotationSpeed * Time.deltaTime * value;
        else
            rotation = 0;

        // Debug.Log("input:" + value + " rotation:" + rotation);
        if (Input.GetButton("Fire") && Time.time > _nextFire)
            Fire();

        /*
            velocity = transform.up * MaxVelocity;

        else
            velocity = Vector3.zero;
        */

        transform.Rotate(0, 0, rotation);
        transform.position += velocity * Time.deltaTime;
        // rotation *= 0.80f * Time.deltaTime;
	}
}
