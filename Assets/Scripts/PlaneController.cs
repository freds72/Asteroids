using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple top-down "plane" controller
/// </summary>
public class PlaneController : MonoBehaviour {

    public GameObject RotationTarget;
    public float RotationSpeed = 1.0f;
    public float Velocity = 1.0f;

    AudioSource _audio;
    float _releaseMemory = 2.0f;
    Dictionary<Stationary.StationaryType, float> _memoryByStation = new Dictionary<Stationary.StationaryType, float>();
    Dictionary<Stationary.StationaryType, int> _releaseCountByStation = new Dictionary<Stationary.StationaryType, int>();

	// Use this for initialization
	void Start () {
	    // listen to stationary events
        Stationaries sts = GetComponent<Stationaries>();
        foreach(var it in sts.Items)
        {
            it.OnRelease += StationRelease;
        }

        _audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
    void Update()
    {
        float lh = Input.GetAxis("LeftStickHorizontal");

        Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
        t.rotation *= Quaternion.Euler(0.0f, 0.0f, -lh * RotationSpeed * Time.deltaTime);

        this.transform.position += Time.deltaTime * Velocity * t.up;
    }

    void StationRelease(Stationary s)
    {
        float memoryTime = 0;
        _memoryByStation.TryGetValue(s.Type, out memoryTime);
        int releaseCount = 0;
        if ( memoryTime > Time.time ) // are we still within time barrier?
            _releaseCountByStation.TryGetValue(s.Type, out releaseCount);
        
        releaseCount++;
        releaseCount = Mathf.Min(releaseCount, 10); // safeguard!

        switch (s.Type)
        {
            case Stationary.StationaryType.Missile:
                AudioClip clip =  Resources.Load<AudioClip>(string.Format("sounds/fox {0}",releaseCount));
                _audio.PlayOneShot(clip);
                break;

            default:
                break;
        }

        // update counters
        _memoryByStation[s.Type] = Time.time + _releaseMemory;
        _releaseCountByStation[s.Type] = releaseCount;
    }
}
