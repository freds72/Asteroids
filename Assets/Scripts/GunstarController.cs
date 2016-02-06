using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(Stationaries))]
[RequireComponent(typeof(Radar))]
public class GunstarController : MonoBehaviour {
    public GameObject RotationTarget;
    public float RotationSpeed = 1.0f;
    public float Velocity = 1.0f;
    public GameObject GunSightPrefab;
    public float GunSightTrackingDelay = 0.2f;
    AudioSource _audio;
    Stationaries _stations;
    Radar _radar;
    bool _missileMode = true;
    GameObject _sight;
    HitPoints _hp;
    Rigidbody _rb;

	// Use this for initialization
	void Start () {
        _radar = GetComponent<Radar>();
        _audio = GetComponent<AudioSource>();
        _stations = GetComponent<Stationaries>();
        _sight = Instantiate(GunSightPrefab, transform.position, Quaternion.identity) as GameObject;
        TargetLink tl = _sight.GetComponent<TargetLink>();
        if (tl != null)
            tl.Target = transform;
        _sight.SetActive(false);

        _hp = GetComponent<HitPoints>();

        // wire stations to gunsight
        foreach(Stationary it in _stations.Items)
        {
            it.Aim = _sight.transform;
        }

        // synchronize input with selected station
        RefreshSelectedStation();
        if ( _stations != null )
        {
            _stations.OnSelectionChanged += (s) =>
            {
                RefreshSelectedStation();
            };
        }

        _rb = GetComponent<Rigidbody>();
	}
	
    void RefreshSelectedStation()
    {
        Stationary selectedStation = _stations.SelectedItem;
        if ( selectedStation != null)
        {
            switch(selectedStation.Type)
            {
                case Stationary.StationaryType.Missile:
                    _radar.enabled = true;
                    _missileMode = true;
                    _sight.SetActive(false);
                    break;
                case Stationary.StationaryType.Gun:
                    _radar.enabled = false;
                    _missileMode = false;
                    _sight.SetActive(true);
                    StartCoroutine(TrackAim());
                    break;
            }
        }
    }

	// Update is called once per frame
    Quaternion _sightRotation = Quaternion.identity;
    float _sightRotationRatio = 0;
	void Update () {
        if (Input.GetButtonUp("WeaponChange"))
            _stations.Next();

        if (_missileMode)
        {
            float lh = Input.GetAxis("LeftStickHorizontal");

            Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
            t.rotation *= Quaternion.Euler(0.0f, 0.0f, -lh * RotationSpeed * Time.deltaTime);
        }
        else // twin stick style
        {
            float lh = Input.GetAxis("LeftStickHorizontal");
            float lv = Input.GetAxis("LeftStickVertical");
            
            //Assumes you're looking down the z axis
            Vector3 move = new Vector3(lh, lv, 0.0f);
            float len = move.sqrMagnitude;
            if (len > 0.15f)
            {
                Vector3 v = Velocity * Time.deltaTime * move.normalized;
                transform.position += v;
              
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _sightRotation, _sightRotationRatio / GunSightTrackingDelay);
            _sightRotationRatio += Time.deltaTime;
        }

        if (Input.GetAxis("Fire") == 1)
            _stations.Release();
	}

    IEnumerator TrackAim()
    {
        while(!_missileMode)
        {            
            Vector3 targetDelta = _sight.transform.position - transform.position;
            targetDelta.z = 0;

            float angle = Mathf.Atan2(targetDelta.y, targetDelta.x) * Mathf.Rad2Deg;
            _sightRotation = Quaternion.Euler(0, 0, angle - 90);
            _sightRotationRatio = 0;

            yield return new WaitForSeconds(GunSightTrackingDelay);
        }
    }

    /*
    void FixedUpdate() 
    {
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, Velocity);
    } 
     * */
}
