using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Defines a weapon, e.g. an object that can inflict damage to another object (usually a HitPoint derived object)
/// </summary>
public class WeaponBehaviour<T> : 
    MonoBehaviour,
    IWeapon 
    where T : struct
{
	// for Unity editor
    public int hitPoints = 1;
    public Enums.DamageType type = Enums.DamageType.Physical;
    public List<T> ignoreTags = new List<T>();

    ITagCollection _tags;
    void Awake()
    {
        _tags = new TagCollection<T>(ignoreTags);
    }
    
#region implements IWeapon
    public int HitPoints { get { return hitPoints; } }
    public Enums.DamageType Type { get { return type; } }
    public ITagCollection IgnoreTags
    {
        get { return _tags; }
    }
#endregion
}