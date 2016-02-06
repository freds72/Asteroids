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

    public List<string> IgnoreTags = new List<string>();

	public void Hit(int hitpoints)
	{
		HP = Mathf.Max(HP - hitpoints,0);
	}
	
	public bool IsDead
	{ get { return HP <= 0; } }
	
	void Start()
	{
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
        Damage dmg = collider.GetComponent<Damage>();
        if (dmg == null)
            return;
        if (IgnoreTags.Contains(collider.tag))
            return;

        Hit(dmg.HitPoints);
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
