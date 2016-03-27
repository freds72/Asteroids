using UnityEngine;
using System.Collections;

/// <summary>
/// Helper class to store all enum definitions
/// </summary>
public static class Enums 
{
    /// <summary>
    /// Weapon damage type
    /// </summary>
    public enum DamageType
    {
        Physical,
        Energy,
        Magical
    };

    public enum PlayerIndex
    {
        Unknown = -1,
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3
    };
}
