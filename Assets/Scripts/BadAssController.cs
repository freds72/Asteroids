using UnityEngine;
using System.Collections;

public class BadAssController : MonoBehaviour {
    Stationaries _stations;

    public GameObject RotationTarget;
    public Transform WeaponTransform;
    public EnemySight ShortRangeSight;
    public EnemySight LongRangeSight;

    private int _shotgunHash = 1;
    private int _miniGunHash = 0;

	// Use this for initialization
	void Start () {
        _stations = GetComponent<Stationaries>();
        SpriteRenderer weaponSprite = WeaponTransform.GetComponent<SpriteRenderer>();
        weaponSprite.sprite = _stations.SelectedItem.Icon;
        _stations.OnSelectionChanged += (s) =>
        {
            weaponSprite.sprite = s.Icon;
        };
	}
    
	// Update is called once per frame
	void Update () {
        bool shoot = false;
        if ( ShortRangeSight.IsPlayerInSight )
        {
            _stations.Select(_shotgunHash);
            shoot = true;
        }
        else if ( LongRangeSight.IsPlayerInSight )
        {
            _stations.Select(_miniGunHash);
            shoot = true;
        }

        if (shoot)
        {
            float rot = 5 * Time.time;

            // set the z axis rotation of this transform in degrees using Euler
            RotationTarget.transform.rotation = Quaternion.Euler(0.0f, rot, 0.0f);

            WeaponTransform.localRotation = Quaternion.Euler(0, 0, -rot);

            //
            _stations.Release();
        }
        else
        {
            WeaponTransform.localRotation = Quaternion.identity;
        }
	}
}
