using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the camera plane in the middle of the player group
/// </summary>
public class PlayerTrackerController : MonoBehaviour {

    public float Velocity = 5;
    public AllTags.Values Tag = AllTags.Values.Player;
    // how much do we prefer player 1?
    public float Player1Stickiness = 0;

	// Use this for initialization
	void Start () {
	
	}

    // LateUpdate called after all objects have been updated
    void LateUpdate()
    {
        int n = 0;
        Vector3 center = Vector3.zero;
        foreach (GameObject it in TagManager.FindAny((long)Tag))
        {
            // TODO: check if player is alive
            center += it.transform.position;
            n++;
        }
        // no more players...
        if (n == 0)
            return;

        // keep original z component
        center.z = transform.position.z;
        // where should we go?
        Vector3 targetDirection = center - transform.position;
        // how fast (based on distance from target)?
        float interpVelocity = targetDirection.magnitude * Velocity;

        Vector3 targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.25f);
	}
}
