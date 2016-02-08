using UnityEngine;
using System.Collections;

public interface ITagCollection 
{
    long Mask { get; }
    GameObject GameObject { get; }
    /// <summary>
    /// Returns whether the other tag collection matches any of tags from self.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool Intersects(ITagCollection other);
}
