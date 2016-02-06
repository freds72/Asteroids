using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    public int HitPoints = 1;
    public enum DamageType
    {
        Physical,
        Energy,
        Magical
    };
    public DamageType Type = DamageType.Physical;
}
