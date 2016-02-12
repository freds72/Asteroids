using UnityEngine;
using System.Collections;
using System;

[Serializable()]
public class RadarMode {
    public string Label = "AA";
    public TagCollection<AllTags.Values> Tags = new TagCollection<AllTags.Values>();
    public float MaxRange = 120;
    public float MinRange = 1;
    public float Angle = 45;
    public float Memory = 0.5f;
    public float ScanPeriod = 0.1f;
}
