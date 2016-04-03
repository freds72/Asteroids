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

    float _shootAngle = 0;
	// Update is called once per frame
	void Update () {
        bool shoot = false;
        if ( ShortRangeSight.IsPlayerInSight )
        {
            _stations.Select(_shotgunHash);
            shoot = true;
            _shootAngle = ShortRangeSight.PlayerAngle;
        }
        else if ( LongRangeSight.IsPlayerInSight )
        {
            _stations.Select(_miniGunHash);
            shoot = true;
            _shootAngle = LongRangeSight.PlayerAngle;
        }

        // weapon direction
        WeaponTransform.localRotation = Quaternion.Euler(0, 0, -_shootAngle);
        RotationTarget.transform.rotation = Quaternion.Euler(0.0f, _shootAngle, 0.0f);

        if (shoot)
            _stations.Release();
	}
}
