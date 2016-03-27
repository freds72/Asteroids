using UnityEngine;
using System.Collections;

/// <summary>
/// Helper class to store all known tags for the game.
/// </summary>
public static class AllTags
{
    public enum Values
    {
        None = 0x0,
        Player1 = 0x1,
        Player2 = 0x2,
        Player3 = 0x4,
        Player4 = 0x8,
        Player = 0x10,
        Enemy = 0x20,
        Emitter = 0x40,
        Launch = 0x80,
        Spawn = 0x100
    };
}
