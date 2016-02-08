using UnityEngine;
using System.Collections;

public class TagCollection<T> : ITagCollection where T: struct
{
    public long Mask
    {
        get;
        private set;
    }

    public GameObject GameObject
    {
        get { throw new System.NotImplementedException(); }
    }

    public bool Intersects(ITagCollection other)
    {
        if (other == null)
            return false;
        return (Mask & other.Mask) != 0L;
    }
}
