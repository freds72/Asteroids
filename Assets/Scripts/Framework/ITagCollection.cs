
using System.Collections;
using System.Collections.Generic;

public interface ITagCollection 
{
    long Mask { get; }
    /// <summary>
    /// Returns whether the other tag collection matches any of tags from self.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool Intersects(ITagCollection other);
    bool Intersects<T>(IEnumerable<T> other) where T: struct;

    /// <summary>
    /// Exact match
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    bool Equals(ITagCollection other);
    bool Equals<T>(IEnumerable<T> other) where T : struct;
}
