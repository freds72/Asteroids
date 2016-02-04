using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stationaries : MonoBehaviour 
{
    public delegate void SelectedEvent(Stationary o);
    public event SelectedEvent OnSelectionChanged;
    public delegate void ReleaseEvent(Stationary s, GameObject go);
    public event ReleaseEvent OnRelease;

    public List<Stationary> Items = new List<Stationary>(4);
    public Radar Radar;
    int _selected = 0;

	// Use this for initialization
	void Start () {
        if (Radar == null)
            Radar = GetComponent<Radar>();
		foreach(Stationary it in Items)
		{
			if ( it.AutoAmmoDelay > 0 )
				StartCoroutine(AutoFill(it));
		}
	}
	
    public Stationary SelectedItem
    {
        get
        {
            if (_selected < Items.Count)
                return Items[_selected];
            return null;
        }
    }

    public void Next()
    {
        int prev = _selected;
        _selected++;
        if (_selected >= Items.Count)
            _selected = 0;

        if (prev != _selected && OnSelectionChanged != null && SelectedItem != null)
            OnSelectionChanged(SelectedItem);
    }

    public void Prev()
    {
        int prev = _selected;
        _selected--;
        if (_selected < 0)
            _selected = Mathf.Max(0, Items.Count - 1);

        if (prev != _selected && OnSelectionChanged != null && SelectedItem != null)
            OnSelectionChanged(SelectedItem);
    }

	// Update is called once per frame
	void Update () {
	    if ( Input.GetButtonUp("Fire"))
        {
            Stationary item = SelectedItem;
            if ( item != null && 
                 item.CanSpawn())
            {
                GameObject go = item.Spawn();
                if (OnRelease != null)
                    OnRelease(item, go);
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

	IEnumerator AutoFill(Stationary s)
	{
		int max = s.Ammo;
		while(true)
		{
			s.Ammo = Mathf.Min(max, s.Ammo + 1);
			yield return new WaitForSeconds(s.AutoAmmoDelay);
		}
	}
}
