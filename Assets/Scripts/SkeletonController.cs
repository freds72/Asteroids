using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SeekController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class SkeletonController : MonoBehaviour {

    public GameObject BlastPrefab;
    public GameObject Avatar;
    public ParticleSystem SmokeParticles;
    public float BlastDistance = 2;

    SeekController _seek;
    Animator _anim;
    Rigidbody _rb;
    SpriteRenderer _renderer;

	// Use this for initialization
	void Start () {
        _renderer = Avatar.GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _seek = GetComponent<SeekController>();
        _seek.OnNearPlayer += (go, d) => {
            if (d < BlastDistance * BlastDistance)
            {
                Instantiate(BlastPrefab, Avatar.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        };
        HitPoints hp = GetComponent<HitPoints>();
        if ( hp != null )
        {
            hp.OnKilled += (go) =>
            {
                SmokeParticles.Stop();
                SmokeParticles.transform.parent = null;
                Destroy(SmokeParticles.gameObject, 3);
            };
        }
	}

    float previousXSign = 0;
    void Update()
    {
        float xsign = Mathf.Sign(_rb.velocity.x);
        if (previousXSign != xsign)
        {
            previousXSign = xsign;
            _anim.SetTrigger("seek");
            _renderer.flipX = _rb.velocity.x < 0;
        }
    }
}
