using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunstarController : MonoBehaviour {
    public GameObject RotationTarget;
    public float RotationSpeed = 1.0f;
    public float Velocity = 1.0f;
    public GameObject GunSightPrefab;
    AudioSource _audio;
    Stationaries _stations;
    Radar _radar;
    bool _missileMode = true;
    GameObject _sight;
    
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
                    break;
            }
        }
    }

	// Update is called once per frame
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
            if (move.sqrMagnitude > 0)
                transform.position += Velocity * Time.deltaTime * move.normalized;
        }
	}
}
