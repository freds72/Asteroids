using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AmmoPanel : MonoBehaviour
{

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
    void Start()
    {

    }

    int _maxAmmo = 0;
    void Refresh()
    {
        _icons.ForEach(i => Destroy(i.gameObject));
        _icons.Clear();

        _maxAmmo = _station.Ammo;
        int slices = IconPrefab.Slices;
        Debug.Log(string.Format("{0} slices: {1}", _station.Name, slices));
        int n = Mathf.CeilToInt(_maxAmmo / slices);
        for (int i = 0; i < n; i++)
        {
            AmmoIcon icon = Instantiate<AmmoIcon>(IconPrefab);
            // ith: slices
            // last: remainder
            icon.Ammo = (i == n - 1) ? (_station.Ammo - i * slices) : slices;
            icon.transform.SetParent(this.transform, false);
            _icons.Add(icon);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        int slices = IconPrefab.Slices;
        // total number of icons
        // ceil
        int n = Mathf.CeilToInt(_maxAmmo / slices);
        // number of full icons
        // floor
        int nfull = Mathf.FloorToInt(ammo / slices);
        // n = 4
        // i = 2
        // 8 - 2*4 = 0
        int remainder = ammo - nfull * slices;

        Debug.Log(_station.Name);
        Debug.Log(string.Format("[{4}/{5}] full: 0-{0} / partial: {1}-{2} -> {3}", nfull - 1, nfull, n, remainder, ammo, _maxAmmo));
        int i = 0;
        for (; i < nfull; i++)
            _icons[i].Ammo = slices;
        // first: remainder
        // ith: 0        
        for (; i < n; i++)
            _icons[i].Ammo = (i == nfull) ? remainder : 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
