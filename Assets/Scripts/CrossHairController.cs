using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TargetLink))]
public class CrossHairController : MonoBehaviour
{
	public float DeadZone = 0.15f;
	public Vector3 Aim
	{ get { return transform.position; } }
	public float RestDistance = 2;
	public float RestDelay = 0.5f;
	public float Scaling = 4;
	
    Transform _parent;
    float _screenWidth;
    float _screenHeight;

	void Start()
	{
        _parent = GetComponent<TargetLink>().Target;
	}

    void Update()
    {
        float rh = Input.GetAxis("RightStickHorizontal");
        float rv = Input.GetAxis("RightStickVertical");
        //Assumes you're looking down the z axis and that you are looking down on the avatar
        Vector3 direction = new Vector3(rh, rv, 0.0f);
        float len = direction.sqrMagnitude;
        if (len > DeadZone * DeadZone)
        {
            transform.position = _parent.position + Scaling * direction;
        }
        /*
        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        // The width is then equal to difference between the rightmost and leftmost x-coordinates
        _screenWidth = screenTopRight.x - screenBottomLeft.x;
        // The height, similar to above is the difference between the topmost and the bottom yycoordinates
        _screenHeight = screenTopRight.y - screenBottomLeft.y;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 0, _screenWidth),
            Mathf.Clamp(transform.position.y, 0, _screenHeight),
            0);
         * */
    }
}