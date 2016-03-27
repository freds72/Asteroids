using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour 
{
    public float ScanDelay = 0.5f;
    // Whether or not the player is currently sighted.
    public bool IsPlayerInSight { get; private set; }
    public Vector3 PlayerPosition {get; private set; }            // Last place this enemy spotted the player.

    private SphereCollider _col;                    // Reference to the sphere collider trigger component.
    private GameObject _player;                      // Reference to the player.

    void Awake()
    {
        // Setting up the references.
        _col = GetComponent<SphereCollider>();
        StartCoroutine(Scan());
    }

    IEnumerator Scan()
    {
        while (true)
        {         
            // locate closest player
            IsPlayerInSight = false;
            _player = TagManager.FindAny((long)AllTags.Values.Player).Closest(transform.position);

            yield return new WaitForSeconds(ScanDelay);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // If the player has entered the trigger sphere...
        if (other.gameObject == _player)
        {
            // By default the player is not in sight.
            IsPlayerInSight = false;

            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - transform.position;
            RaycastHit hit;

            // ... and if a raycast towards the player hits something...
            if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out hit, _col.radius))
            {
                // ... and if the raycast hits the player...
                if (hit.collider.gameObject == _player)
                {
                    // ... the player is in sight.
                    IsPlayerInSight = true;
                    PlayerPosition = _player.transform.position;
                }
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject == _player)
            // ... the player is not in sight.
            IsPlayerInSight = false;
    }
}
