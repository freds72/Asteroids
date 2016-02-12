using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class TagCollection<T> :
    ITagCollection
    where T : struct
{
    public List<T> Tags;
    
    public TagCollection()
    {
        Tags = new List<T>();
    }

    public TagCollection(List<T> tags)
    {
        Tags = tags;
    }

    void UpdateMask()
    {
        foreach (T it in Tags)
            _mask |= Convert.ToInt64(it);
        _locked = true;
    }
    bool _locked = false;
    long _mask = 0;
    public long Mask
    {
        get
        {
            if (!_locked)
                UpdateMask();
            return _mask;
        }
    }

    public bool Intersects(ITagCollection other)
    {
        if (other == null)
            return false;
        return (Mask & other.Mask) != 0L;
    }

    public bool Equals(ITagCollection other)
    {
        if (other == null)
            return false;
        return (Mask == other.Mask);
    }

    public static explicit operator Int64(TagCollection<T> x)
    {
        if (x == null) throw new InvalidCastException();
        return x.Mask;
    }
}
