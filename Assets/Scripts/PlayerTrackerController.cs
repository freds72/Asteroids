using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the camera plane in the middle of the player group
/// </summary>
public class PlayerTrackerController : MonoBehaviour {

    public float Velocity = 5;
    public string Tag = "Player";

	// Use this for initialization
	void Start () {
	
	}

    // LateUpdate called after all objects have been updated
    void LateUpdate()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tag);
        if ( players.Length == 0 )
            return; // nothing to track

        int n = 0;
        Vector3 center = Vector3.zero;
        foreach (GameObject it in players)
        {
            // TODO: check if player is alive
            center += it.transform.position;
            n++;
        }

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
