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
	
	int _isAiming = 0;
    Transform _parent;

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
            // stop rest shift
            _isAiming++;
            transform.position = _parent.position + Scaling * direction;
        }
            /*
        else
        {
            _isAiming++;
            StartCoroutine(GoToRest());
        } */
    }

	/*
	IEnumerator GoToRest()
	{
		int flag = _isAiming;
		float now = Time.time;
        Vector3 sourcePosition = transform.position;
        float elapsedTime = Vector3.Distance()
		// move to rest position unless cancelled		
        while(_isAiming==flag)
		{
            transform.position = Vector3.Lerp(sourcePosition, , Mathf.SmoothStep(0, 1, (elapsedTime / RestDelay)));
            elapsedTime += Time.deltaTime;
		}
	}
     * */
}