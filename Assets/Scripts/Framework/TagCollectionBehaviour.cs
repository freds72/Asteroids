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
    where T: struct
{
	public List<T> Tags = new List<T>();
	
	void Awake()
	{
        foreach (T it in Tags)
            Mask |= Convert.ToInt64(it);

		TagManager.Self.Register(this);
	}

	void OnDestroy()
	{
		TagManager.Self.Unregister(this);
	}

    /// <summary>
    /// Bitmask for tags
    /// </summary>
    public long Mask
    {
        get;
        private set;
    }

    /// <summary>
    /// Returns the underlying gameobject
    /// </summary>
    public GameObject GameObject
    {
        get { return gameObject; }
    }

    public bool Intersects(ITagCollection other)
    {
        if (other == null)
            return false;
        return (Mask & other.Mask) != 0L;
    }
}
