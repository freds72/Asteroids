using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunstarController : MonoBehaviour {
    public GameObject RotationTarget;
    public float RotationSpeed = 1.0f;
    public float Velocity = 1.0f;
    public GameObject GunSightPrefab;
    public List<Transform> GunAnchors;
    public List<Transform> MissileAnchors;
    AudioSource _audio;
    Stationaries _stations;
    Radar _radar;
    bool _useTwinStick = true;
    GameObject _sight;
    
	// Use this for initialization
	void Start () {
        _radar = GetComponent<Radar>();
        _audio = GetComponent<AudioSource>();
        _stations = GetComponent<Stationaries>();
        _sight = Instantiate(GunSightPrefab, transform.position, Quaternion.identity) as GameObject;
        _sight.SetActive(false);

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
                    _useTwinStick = false;
                    _sight.SetActive(false);
                    break;
                case Stationary.StationaryType.Gun:
                    _radar.enabled = false;
                    _useTwinStick = true;
                    _sight.SetActive(true);
                    break;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp("WeaponChange"))
            _stations.Next();

        if (!_useTwinStick)
        {
            float lh = Input.GetAxis("LeftStickHorizontal");

            Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
            t.rotation *= Quaternion.Euler(0.0f, 0.0f, -lh * RotationSpeed * Time.deltaTime);
        }
        else
        {
            float lh = Input.GetAxis("LeftStickHorizontal");
            float lv = Input.GetAxis("LeftStickVertical");
            float rh = Input.GetAxis("RightStickHorizontal");
            float rv = Input.GetAxis("RightStickVertical");

            //Assumes you're looking down the z axis
            Vector3 move = new Vector3(lh, lv, 0.0f);
            if (move.sqrMagnitude > 0)
                transform.position += Velocity * Time.deltaTime * move.normalized;
            //Assumes you're looking down the z axis and that you are looking down on the avatar
            Vector3 direction = new Vector3(rh, rv, 0.0f);
            if (direction.sqrMagnitude > 0.1f)
            {
                direction.Normalize();
                // get the angle of the difference in radians, then convert to degrees using Rad2Deg
                float zrot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // set the z axis rotation of this transform in degrees using Euler
                Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
                t.rotation = Quaternion.Euler(0.0f, 0.0f, zrot - 90);
            }
        }
	}
}
