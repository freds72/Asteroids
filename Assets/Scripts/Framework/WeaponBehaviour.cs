using UnityEngine;
using System.Collections;

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
    public TagCollection<T> ignoreTags = new TagCollection<T>();

#region implements IWeapon
    public int HitPoints { get { return hitPoints; } }
    public Enums.DamageType Type { get { return type; } }
    public ITagCollection IgnoreTags
    {
        get { return ignoreTags; }
    }
#endregion
}