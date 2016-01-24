using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stationaries : MonoBehaviour {
    public List<Stationary> Items = new List<Stationary>(4);
    public Radar Radar;
    int _selected = 0;

	// Use this for initialization
	void Start () {
        if (Radar == null)
            Radar = GetComponent<Radar>();
	}
	
    Stationary SelectedItem
    {
        get
        {
            if (_selected < Items.Count)
                return Items[_selected];
            return null;
        }
    }

	// Update is called once per frame
	void Update () {
	    if ( Input.GetButtonUp("Fire"))
        {
            Stationary item = SelectedItem;
            if ( item != null && 
                 item.CanSpawn())
            {
                GameObject go = item.Spawn(transform);
                Transform tgt = Radar.AcquireLock;
                if ( tgt != null )
                {
                    // get the target interface
                    TargetLink link = go.GetComponent<TargetLink>();
                    link.Target = tgt;
                }
            }
        }
	}
}
