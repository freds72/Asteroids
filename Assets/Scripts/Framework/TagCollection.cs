using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

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

    public bool Intersects<C>(IEnumerable<C> other) where C : struct
    {
        if (other == null)
            return false;
        foreach (C it in other)
        {
            Debug.Log(string.Format("{0} & {1} = {2}", Mask, Convert.ToInt64(it), (Mask & Convert.ToInt64(it))));
            if ((Mask & Convert.ToInt64(it)) != 0)
                return true;
        }
        return false;
    }

    public bool Equals<C>(IEnumerable<C> other) where C : struct
    {
        if (other == null)
            return false;
        long otherMask = 0L;
        foreach (C it in other)
            otherMask |= Convert.ToInt64(it);
        return (Mask == otherMask);
    }

    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        foreach (T it in Tags)
        {
            s.Append(it.ToString());
            s.Append("\n");
        }
        return s.ToString();
    }
}
