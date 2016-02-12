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
        Player3 = 0x3,
        Player4 = 0x4,
        Player = 0x5,
        Enemy = 0x6,
        Emitter = 0x7,
        Launch = 0x8,
        Spawn = 0x9
    };
}
