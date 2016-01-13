using UnityEngine;
using System.Collections;

public class RadarItem
{
    public RadarSignature Target { get; private set; }
    public float LastFiredAtTime { get; set; }
    public float LastSeenTime { get; set; }
    public bool IsLocked { get; set; }
    public bool IsSelected { get; set; }

    public RadarItem(RadarSignature target)
    {
        LastSeenTime = Time.time;
        target.Spotted++;
        Target = target;
    }

    public void Release()
    {
        // Unity does some magic with managed wrappers when object is destroyed
        if (Target != null)
            Target.Spotted--;
    }
}
