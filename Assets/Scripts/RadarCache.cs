using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class RadarCache : IEnumerable<RadarItem>
{
    public RadarMode Mode { get; private set; }
    public Transform Host { get; private set; }
    float _refreshTime = 0;
    float _angle;
    float _maxSqrd;
    float _minSqrd;
    Dictionary<int, RadarItem> _blips = new Dictionary<int, RadarItem>(8);
    SortedList<float, RadarItem> _blipsByDistance = new SortedList<float, RadarItem>();
    // keep both selected index and selected instance ID for consistency
    int _selectedIndex = 0;

    public RadarCache(Transform host, RadarMode mode)
    {
        Host = host;
        Mode = mode;
        _refreshTime = Time.time;
        _maxSqrd = mode.MaxRange * mode.MaxRange;
        _minSqrd = mode.MinRange * mode.MinRange;
        _angle = Mathf.Sin(Mathf.Deg2Rad * mode.Angle);
    }

    public void Pause()
    {
        foreach(RadarItem it in this)
        {
            it.Release();
        }
        // TODO: acquire again the target when cache is enabled again (LastSeentime && released-> spotted ++ && released = false)
    }

    public void Update()
    {
        if (Time.time >= _refreshTime)
        {
            _refreshTime = Time.time + Mode.ScanPeriod;
            Scan();
        }
    }

    List<int> _lostIds = new List<int>(8);
    void Scan()
    {
        // get current selection
        RadarItem currentSelection = SelectedItem;
        int selectedID = -1;
        if ( currentSelection != null )
            selectedID = currentSelection.InstanceID;

        // clear previous sort
        _blipsByDistance.Clear();
        _blipsByDistance.Clear();
        _selectedIndex = 0;
        // Find all gameobjects in range
        IEnumerator<KeyValuePair<float, GameObject>> it = FindBlips();
        while (it.MoveNext())
        {
            RadarSignature rs = it.Current.Value.GetComponent<RadarSignature>();
            if (rs != null)
            {
                // is it a known target?
                int id = it.Current.Value.GetInstanceID();
                RadarItem blip = null;
                if (!_blips.TryGetValue(id, out blip))
                {
                    blip = new RadarItem(rs);
                    _blips.Add(id, blip);
                }
                else
                {
                    blip.LastSeenTime = Time.time;
                }
                // sort all blips by distance
                // Debug.Log(string.Format("{0}: {1} kms", blip.Target.name, Mathf.Sqrt(it.Current.Key)));
                blip.Distance = it.Current.Key;
                _blipsByDistance.Add(it.Current.Key, blip);
            }
        }
        // refresh selected position
        for (int i = 0; i < _blipsByDistance.Count; i++)
        {
            if (_blipsByDistance.Values[i].InstanceID == selectedID)
            {
                _selectedIndex = i;
                break;
            }
        }
        // cleanup 'lost' blips
        _lostIds.Clear();
        Dictionary<int, RadarItem>.Enumerator itid = _blips.GetEnumerator();
        while(itid.MoveNext())
        {
            if ( itid.Current.Value.LastSeenTime + Mode.Memory < Time.time)
            {
                itid.Current.Value.Release();
                _lostIds.Add(itid.Current.Key);
            }
        }
        foreach (int id in _lostIds)
            _blips.Remove(id);
    }

    IEnumerator<KeyValuePair<float, GameObject>> FindBlips()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(Mode.Tag);
        Vector3 position = Host.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < _maxSqrd && curDistance > _minSqrd)
            {
                // check against radar cone
                diff.Normalize();
                if (Vector3.Dot(Host.transform.up, diff) >= _angle)
                {
                    yield return new KeyValuePair<float,GameObject>(Mathf.Sqrt(curDistance), go);
                }
            }
        }
    }

    public IEnumerator<RadarItem> GetEnumerator()
    {
        if ( _blips.Count == 0)
            return Enumerable.Empty<RadarItem>().GetEnumerator();
        return _blips.Values.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        if (_blips.Count == 0)
            return Enumerable.Empty<RadarItem>().GetEnumerator();
        return _blips.Values.GetEnumerator();
    }

    /// <summary>
    /// Return any locked item and unlock it
    /// </summary>
    public RadarItem AcquireLock
    {
        get
        {
            RadarItem item = _blipsByDistance.FirstOrDefault(it => it.Value.IsLocked).Value;
            if (item != null)
                item.IsLocked = false;
            return item;
        }
    }

    public RadarItem SelectedItem
    {
        get
        {
            if (_selectedIndex < _blipsByDistance.Count && _selectedIndex >= 0)
                return _blipsByDistance.Values[_selectedIndex];
            return null;
        }
    }

    public void Next()
    {
        _selectedIndex++;
        if (_selectedIndex >= _blipsByDistance.Count)
            _selectedIndex = 0;
    }

    public void Previous()
    {
        _selectedIndex--;
        if (_selectedIndex < 0)
            _selectedIndex = Mathf.Max(0,_blipsByDistance.Count - 1);
    }

    public void ToggleLock()
    {
        RadarItem ri = SelectedItem;
        if (ri != null)
            ri.IsLocked = !ri.IsLocked;
    }
}