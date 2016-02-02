using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AmmoPanel : MonoBehaviour {

    public AmmoIcon IconPrefab;

    Stationary _station;
    public Stationary Station
    {
        get { return _station; }
        set
        {
            _station = value;
            Refresh();
        }
    }
    List<AmmoIcon> _icons = new List<AmmoIcon>();

	// Use this for initialization
	void Start () {
               
	}

    void Refresh()
    {
        _icons.ForEach(i => Destroy(i));
        _icons.Clear();

        int slices = IconPrefab.Slices;
        int n = Mathf.RoundToInt(_station.Ammo / slices + 0.5f);
        for(int i=0;i<n;i++)
        {
            AmmoIcon icon = Instantiate<AmmoIcon>(IconPrefab);
            icon.Ammo = 4; // TODO:
            icon.transform.SetParent(this.transform, false);
            _icons.Add(icon);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
