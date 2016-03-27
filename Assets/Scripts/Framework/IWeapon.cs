using UnityEngine;
using System.Collections;

public interface IWeapon 
{
    int HitPoints { get; }
    Enums.DamageType Type { get; }
    ITagCollection IgnoreTags { get; }
}
