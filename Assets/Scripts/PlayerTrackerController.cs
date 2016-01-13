using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the camera plane in the middle of the player group
/// </summary>
public class PlayerTrackerController : MonoBehaviour {

    public string Tag = "Player";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
        // TODO: smooth lerp to new position
        transform.position = new Vector3(center.x / n, center.y / n, transform.position.z);
	}
}
