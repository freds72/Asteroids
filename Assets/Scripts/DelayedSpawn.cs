using UnityEngine;
using System.Collections;

public class DelayedSpawn : MonoBehaviour {

    public GameObject Prefab;
    public float Delay = 1;
    public bool AutoDestroy = true;

	// Use this for initialization
	void Start () {
        Invoke("Spawn", Delay);
	}
	
    void Spawn()
    {
        Instantiate(Prefab, transform.position, transform.rotation);
        if (AutoDestroy)
        {
            // no longer needed, suicide...
            Destroy(gameObject);
        }
    }
}
