using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class EnemySight : MonoBehaviour 
{
    public float ScanDelay = 0.5f;
    // Whether or not the player is currently sighted.
    public bool IsPlayerInSight { get; private set; }
    public Vector3 PlayerPosition {get; private set; }            // Last place this enemy spotted the player.
    /// <summary>
    /// Returns the angle between the GameObject and the player
    /// </summary>
    public float PlayerAngle { get; private set; }
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
        if (_player != null && 
            other.gameObject == _player)
        {
            // By default the player is not in sight.
            IsPlayerInSight = false;

            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = (other.transform.position - transform.position).normalized;
            RaycastHit hit;

            // ... and if a raycast towards the player hits something..
            int layerMask = 1 << 13;
            layerMask = ~layerMask;
            if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, _col.radius, layerMask))
            {
                // ... and if the raycast hits the player...
                if (hit.collider.gameObject == _player)
                {
                    // ... the player is in sight.
                    IsPlayerInSight = true;
                    PlayerPosition = _player.transform.position;
                    PlayerAngle = 360 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
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
