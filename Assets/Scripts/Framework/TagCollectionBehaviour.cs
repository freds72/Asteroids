using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Supports for multiple tags attached to a GameObject.
/// </summary>
/// <remarks>
/// The class must be derived to use the right set of tags.
/// </remarks>
public abstract class TagCollectionBehaviour<T> :
    MonoBehaviour,
    ITagCollection
    where T:struct
{
    public List<T> Tags = new List<T>();

    ITagCollection _tags;
    void Awake()
    {
        _tags = new TagCollection<T>(Tags);
        TagManager.Register(_tags, gameObject);
    }

    void OnDestroy()
    {
        TagManager.Unregister(_tags, gameObject);
    }

    public long Mask { get { return _tags.Mask; } }

    public bool Intersects(ITagCollection other)
    {
        return _tags.Intersects(other);
    }

    public bool Equals(ITagCollection other)
    {
        return _tags.Equals(other);
    }

    public bool Intersects<C>(IEnumerable<C> other) where C : struct
    {
        return _tags.Intersects(other);
    }

    public bool Equals<C>(IEnumerable<C> other) where C : struct
    {
        return _tags.Equals(other);
    }

    public override string ToString()
    {
        return _tags.ToString();
    }
}
