using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// A radar attached to a GameObject
public class Radar : MonoBehaviour,IEnumerable<RadarItem>
{
    public List<RadarMode> Modes = new List<RadarMode>();
    List<RadarCache> _cache = new List<RadarCache>();
    public int DefaultMode = 0;
    int _selectedMode = 0;
    // Use this for initialization
    void Start()
    {
        _selectedMode = DefaultMode;
        foreach (RadarMode it in Modes)
            _cache.Add(new RadarCache(this, it));
    }

    void ChangeMode(int mode)
    {
        // pause scan of previous mode
        if (_selectedMode != mode)
            _cache[_selectedMode].Pause();
        _selectedMode = mode;
    }

    public void NextMode()
    {
        ChangeMode(Mathf.Min(_selectedMode + 1, Modes.Count - 1));
    }

    public void PreviousMode()
    {
        ChangeMode(Mathf.Max(_selectedMode - 1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Modes.Count == 0)
            return; // nothing to do
        _cache[_selectedMode].Update();
    }

    public IEnumerator<RadarItem> GetEnumerator()
    {
        if (_cache.Count == 0)
           return Enumerable.Empty<RadarItem>().GetEnumerator();
        return _cache[_selectedMode].GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        if (_cache.Count == 0)
            return Enumerable.Empty<RadarItem>().GetEnumerator();
        return _cache[_selectedMode].GetEnumerator();
    }
}
