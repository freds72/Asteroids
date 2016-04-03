using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class ImpController : MonoBehaviour {
    public GameObject Avatar;
    public GameObject RotationTarget;
    public EnemySight ShortRangeSight;
    public EnemySight LongRangeSight;
    public float LongRangeVelocity = 1;
    public float ShortRangeVelocity = 5;

    SpriteRenderer _renderer;
    Rigidbody _rb;
    Stationaries _stations;
    SeekController _seek;
    private int _fireballHash = 0;

	// Use this for initialization
	void Start () {
        _seek = GetComponent<SeekController>();
        _renderer = Avatar.GetComponent<SpriteRenderer>();
        _stations = GetComponent<Stationaries>();
        _rb = GetComponent<Rigidbody>();
	}

    float _shootAngle = 0;
    void Update()
    {
        // default speed
        _seek.Velocity = LongRangeVelocity;
        bool shoot = false;
        if (ShortRangeSight.IsPlayerInSight)
        {
            /* _stations.Select(_shotgunHash);
             
            shoot = true;
            */
            _shootAngle = ShortRangeSight.PlayerAngle;
            _seek.Velocity = ShortRangeVelocity;
        }
        else if (LongRangeSight.IsPlayerInSight)
        {
            _stations.Select(_fireballHash);
            shoot = true;
            _shootAngle = LongRangeSight.PlayerAngle;
        }

        // weapon direction
        RotationTarget.transform.rotation = Quaternion.Euler(0.0f, _shootAngle, 0.0f);

        if (shoot)
            _stations.Release();

        _renderer.flipX = _rb.velocity.x < 0;
    }
}
