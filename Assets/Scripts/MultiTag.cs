using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Supports for multiple tags attached to a GameObject.
/// </summary>
public class MultiTag : MonoBehaviour
{
	public List<string> Tags = new List<string>();
	
	void Awake()
	{
        TagManager.Self.Register(gameObject.tag, gameObject);
		TagManager.Self.Register(this);
	}
	void OnDestroy()
	{
		TagManager.Self.Unregister(this);
        TagManager.Self.Unregister(gameObject.tag, gameObject);
	}
}
