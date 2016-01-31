using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

// A radar attached to a GameObject
public class NPCRadar : 
    MonoBehaviour, 
    IEnumerable<RadarItem>
{
    public RadarMode Specs = new RadarMode();
    public RadarMode EyeSpecs = new RadarMode() { Label = "Eye", Angle = 120, MaxRange = 5, MinRange = 0, Memory = 5.0f, ScanPeriod = 1.0f };
    
    RadarCache _cache;
    
    RadarCache _eyeCache;
  
    // Use this for initialization
    void Start()
    {
        _cache = new RadarCache(transform, Specs);

        _eyeCache = new RadarCache(transform, EyeSpecs);
    }

    // Update is called once per frame
    void Update()
    {
        _eyeCache.Update();

        _cache.Update();
    }
		
    public IEnumerator<RadarItem> GetEnumerator()
    {
        return _cache.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _cache.GetEnumerator();
    }
}
