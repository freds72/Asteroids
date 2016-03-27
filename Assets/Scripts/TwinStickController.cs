using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TwinStickController : MonoBehaviour {

    public float MaxVelocity = 3.0f;
    public float WalkVelocityFloor = 0.1f;
    public GameObject RotationTarget;
    public Transform WeaponTransform;
    public AudioClip WeaponChangeClip;

    Rigidbody _rb;
    AudioSource _audio;
    Vector3 _move;
    Animator _anim;
    int _walkTrigger = Animator.StringToHash("walking");
    Stationaries _stations;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _stations = GetComponent<Stationaries>();
        Player player = GetComponent<Player>();
        _stations.OnRelease += (s, go) =>
        {
            // assign player id to any weapon being fired
            IPlayerIndex idx = go.GetComponent<IPlayerIndex>();
            if (idx != null)
                idx.PlayerIndex = player.PlayerIndex;
        };
        SpriteRenderer weaponSprite = WeaponTransform.GetComponent<SpriteRenderer>();
        _stations.OnSelectionChanged += (s) =>
            {
                weaponSprite.sprite = s.Icon;
                if ( WeaponChangeClip != null)
                    _audio.PlayOneShot(WeaponChangeClip);
            };
	}
	
	// Update is called once per frame
    void Update()
    {
        float lh = Input.GetAxis("LeftStickHorizontal");
        float lv = Input.GetAxis("LeftStickVertical");
        float rh = Input.GetAxis("RightStickHorizontal");
        float rv = Input.GetAxis("RightStickVertical");

        //Assumes you're looking down the z axis
        _move = new Vector3(lh, 0, lv);
        if (_move.sqrMagnitude < 0.1f)
            _move = Vector3.zero;

        //Assumes you're looking down the z axis and that you are looking down on the avatar
        Vector3 direction = new Vector3(rh, rv, 0.0f);
        if (direction.sqrMagnitude > 0.1f)
        {
            direction.Normalize();
            // get the angle of the difference in radians, then convert to degrees using Rad2Deg
            float rot = 360 - Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // set the z axis rotation of this transform in degrees using Euler
            Transform t = RotationTarget == null ? this.transform : RotationTarget.transform;
            t.rotation = Quaternion.Euler(0.0f, rot, 0.0f);

            WeaponTransform.localRotation = Quaternion.Euler(0, 0, -rot);
        }
        else
        {
            WeaponTransform.localRotation = Quaternion.identity;
        }

        if (Input.GetButtonUp("WeaponChange"))
            _stations.Next();

        if (Input.GetAxis("Fire") == 1)
            _stations.Release();
    }

    void FixedUpdate()
    {
        _rb.AddForce(_move, ForceMode.Impulse);
        Vector3 v = _rb.velocity;
        if (v.sqrMagnitude > MaxVelocity * MaxVelocity)
            _rb.velocity = Vector3.ClampMagnitude(v, MaxVelocity);

        _anim.SetBool(_walkTrigger, _rb.velocity.sqrMagnitude > WalkVelocityFloor * WalkVelocityFloor);
    }
}
