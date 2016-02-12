using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HitPoints : MonoBehaviour
{
	public int HP = 100;
	public int MaxHP = 100;
	// hp per seconds
	public float RegenerationRate = 0;
	// prefab to spawn when dying
	public GameObject DiePrefab;
	// safe delay upon spawn
	public float SafeDelay = 0;
	bool _isSafe = false;
	Animator _anim;
	static int _hitTrigger = Animator.StringToHash("hit");
	static int _safeTrigger = Animator.StringToHash("safe");
	
	public void Hit(int hitpoints)
	{
		if (_isSafe)
			return;
		HP = Mathf.Max(HP - hitpoints,0);
		if (_anim)
			_anim.SetTrigger(_hitTrigger);
	}
	
	public bool IsDead
	{ get { return HP <= 0; } }

    ITagCollection _tags;
	void Start()
	{
        _tags = GetComponent<ITagCollection>();
        _anim = GetComponent<Animator>();
        if (SafeDelay > 0)
        	StartCoroutine(SafeState());
        
	}
	
	IEnumerator SafeState()
	{
		_isSafe = true;
		if (_anim)
			_anim.SetTrigger(_safeTrigger);
		yield return new WaitForSeconds(SafeDelay);
		_isSafe = false;
	}
	
	float _bonus = 0;
	void LateUpdate()
	{
		_bonus += Time.deltaTime * RegenerationRate;
		int health = Mathf.RoundToInt(_bonus);
		_bonus -= health;
		HP = Mathf.Min(HP + health, MaxHP);

        // are we down?
        if (IsDead)
        {
            Destroy(gameObject);
            if (DiePrefab != null)
                Instantiate(DiePrefab, transform.position, transform.rotation);
        }
	}

    void DoCollision(GameObject collider)
    {
        IWeapon wp = collider.GetComponent<IWeapon>();
        if (wp == null)
            return;
        
        if (wp.IgnoreTags.Intersects(_tags))
            return;

        Hit(wp.HitPoints);
    }

    void OnCollisionEnter(Collision col)
    {
        DoCollision(col.collider.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        DoCollision(other.gameObject);
    }
}
