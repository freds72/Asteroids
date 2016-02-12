using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

// A simple stack of locked Transform's
public class Radar : MonoBehaviour
{
    public GameObject LockPrefab;

    List<GameObject> _locks = new List<GameObject>();
    Dictionary<GameObject, Transform> _locksByTarget= new Dictionary<GameObject, Transform>();
    List<Transform> _locksInstanceCache = new List<Transform>(4);
    int _lastSelectedItem = -1;

    // Use this for initialization
    void Start()
    {
      
    }
    
    public void PushLock(GameObject go)
    {
    	// already there?
    	if (!_locks.Contains(go))
    		_locks.Add(go);
    }

    void OnDisable()
    {
       foreach(Transform it in _locksInstanceCache)
       	Destroy(it.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // remove destroyed objects

        // update target lock position
        int i = 0;
        foreach (GameObject it in _locks)
        {
            Transform lck = null;
            if (!_locksByTarget.TryGetValue(it.gameObject, out lck))
            {
                // pick lock target from cache
                if (i >= _locksInstanceCache.Count)
                {
                    // create a new instance
                    Transform newLock = Instantiate(LockPrefab).GetComponent<Transform>();
                    newLock.gameObject.SetActive(false);
                    _locksInstanceCache.Add(newLock);
                }
                lck = _locksInstanceCache[i];
                if (!lck.gameObject.activeInHierarchy)
                    lck.gameObject.SetActive(true);
                i++; _locksByTarget[it.gameObject] = lck;
            }
            lck.position = it.transform.position;
        }
        // disable remaining locks
        for (; i < _locksInstanceCache.Count; i++)
        {
            Transform lck = _locksInstanceCache[i];
            if (lck.gameObject.activeInHierarchy)
                lck.gameObject.SetActive(false);
        }
    }

    public Transform PopLock()
    {
        GameObject go = null;
        if (_locks.Count > 0)
        {
            int i = _locks.Count - 1;
            go = _locks[i];
            _locks.RemoveAt(i);
        }
        return go==null?null:go.transform;
    }
}
