using System;
using UnityEngine;
using System.Collections.Generic;

class RadarCache : IEnumerable<RadarItem>
{
    public RadarMode Mode { get; private set; }
    public Radar Radar { get; private set; }
    float _refreshTime = 0;
    float _angle;
    float _maxSqrd;
    float _minSqrd;
    Dictionary<int, RadarItem> _blips = new Dictionary<int, RadarItem>(8);

    public RadarCache(Radar radar, RadarMode mode)
    {
        Radar = radar;
        Mode = mode;
        _refreshTime = Time.time;
        _maxSqrd = mode.MaxRange * mode.MaxRange;
        _minSqrd = mode.MinRange * mode.MinRange;
        _angle = Mathf.Sin(mode.Angle);
        if (_angle < 0)
            throw new ArgumentOutOfRangeException("Radar angle is invalid: " + mode.Angle);
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
        // Find all gameobjects in range
        IEnumerator<GameObject> it = FindBlips();
        while (it.MoveNext())
        {
            RadarSignature rs = it.Current.GetComponent<RadarSignature>();
            if (rs != null)
            {
                // is it a known target?
                int id = it.Current.GetInstanceID();
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

    IEnumerator<GameObject> FindBlips()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(Mode.Tag);
        Vector3 position = Radar.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < _maxSqrd && curDistance > _minSqrd)
            {
                // check against radar cone
                diff.Normalize();
                if (Vector3.Dot(Radar.transform.up, diff) >= _angle)
                {
                    yield return go;
                }
            }
        }
    }

    public IEnumerator<RadarItem> GetEnumerator()
    {
        return _blips.Values.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
       return _blips.Values.GetEnumerator();
    }
}