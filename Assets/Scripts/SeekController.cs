using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SeekController : MonoBehaviour {

    public delegate void NearPlayer(GameObject go, float sqrDist);
    public event NearPlayer OnNearPlayer;

    public float Velocity = 2;
    public float ThinkDelay = 0.3f;
    public float MinRadius = 1;
    Rigidbody _rb;
    Vector3 _dir = Vector3.zero;
	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(TrackPlayer());
        StartCoroutine(AvoidOthers());
	}

    IEnumerator TrackPlayer()
    {
        while (true)
        {
            Vector3 position = transform.position;
            GameObject target = null;
            Vector3 targetDir = Vector3.zero;
            float nearestSqrDist = float.MaxValue;
            _dir = Vector3.zero;
            foreach (GameObject it in TagManager.FindAny((long)AllTags.Values.Player))
            {
                Vector3 dir = it.transform.position - position;
                float dist = dir.sqrMagnitude;
                if (dist < nearestSqrDist)
                {
                    nearestSqrDist = dist;
                    target = it;
                    targetDir = dir;
                }
            }
            if (target != null)
            {
                _dir = new Vector3(targetDir.x, 0, targetDir.z);
                _dir.Normalize();

                if (OnNearPlayer != null)
                    OnNearPlayer(target, nearestSqrDist);
            }

            yield return new WaitForSeconds(ThinkDelay);
        }
    }

    IEnumerator AvoidOthers()
    {
        while (true)
        {
            // avoid other enemies
            Vector3 position = transform.position;
            GameObject target = null;
            Vector3 targetDir = Vector3.zero;
            float nearestSqrDist = MinRadius * MinRadius;
            foreach (GameObject it in TagManager.FindAny((long)AllTags.Values.Enemy))
            {
                // skip self
                if (it == gameObject)
                    continue;

                Vector3 dir = position - it.transform.position;
                float dist = dir.sqrMagnitude;
                if (dist < nearestSqrDist)
                {
                    nearestSqrDist = dist;
                    target = it;
                    targetDir = dir;
                }
            }
            if (target != null)
            {
                _dir += (new Vector3(targetDir.x, 0, targetDir.z)).normalized;
                _dir.Normalize();
            }

            yield return new WaitForSeconds(ThinkDelay);
        }
	}

    void FixedUpdate()
    {
        _rb.AddForce(Velocity * _dir, ForceMode.Impulse);
        Vector3 v = _rb.velocity;
        if (v.sqrMagnitude > Velocity * Velocity)
            _rb.velocity = Vector3.ClampMagnitude(v, Velocity);
    }
}
