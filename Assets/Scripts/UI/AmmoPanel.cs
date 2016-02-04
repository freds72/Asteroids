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

    int _maxAmmo = 0;
    void Refresh()
    {
        _icons.ForEach(i => DestroyObject(i));
        _icons.Clear();

        _maxAmmo = _station.Ammo;
        int slices = IconPrefab.Slices;
        int n = Mathf.RoundToInt(_maxAmmo / slices + 0.5f);
        for(int i=0;i<n;i++)
        {
            AmmoIcon icon = Instantiate<AmmoIcon>(IconPrefab);
            // ith: slices
            // last: remainder
            icon.Ammo = (i==n-1)?(_station.Ammo - i*slices):slices;
            icon.transform.SetParent(this.transform, false);
            _icons.Add(icon);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        int slices = IconPrefab.Slices;
        int n = Mathf.RoundToInt(_maxAmmo / slices + 0.5f);
        int i = Mathf.RoundToInt(ammo / slices);
        for (; i < n; i++)
        {
            _icons[i].Ammo = (i==n-1)?(_maxAmmo - i*slices):slices;
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
