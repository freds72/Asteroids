using UnityEngine;
using System.Collections;

public class TwinStickController : MonoBehaviour {

    public float Sensitivity = 3.0f;
    public GameObject RotationTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        float lh = Input.GetAxis("LeftStickHorizontal");
        float lv = Input.GetAxis("LeftStickVertical");
        float rh = Input.GetAxis("RightStickHorizontal");
        float rv = Input.GetAxis("RightStickVertical");


        //Assumes you're looking down the z axis
        Vector3 move = new Vector3(lh, lv, 0.0f);
        if ( move.sqrMagnitude > 0 )
            transform.position += Sensitivity * Time.deltaTime * move.normalized;
        //Assumes you're looking down the z axis and that you are looking down on the avatar
        Vector3 direction = new Vector3(rh, rv, 0.0f);
        if (direction.sqrMagnitude > 0.1f)
        {
            direction.Normalize();
            // get the angle of the difference in radians, then convert to degrees using Rad2Deg
            float zrot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // set the z axis rotation of this transform in degrees using Euler
            Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
            t.rotation = Quaternion.Euler(0.0f, 0.0f, zrot - 90);
        }
    }
}
