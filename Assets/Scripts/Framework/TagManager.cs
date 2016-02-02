using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TagManager
{
	public static readonly TagManager Self = new TagManager();
	
	Dictionary<string, Dictionary<int, GameObject>> _goByTag = new Dictionary<string, Dictionary<int, GameObject>>();
	
	public void Register(string tag, GameObject go)
	{
		Dictionary<int, GameObject> gos = null;
		if (!_goByTag.TryGetValue(tag,out gos))
		{
			gos = new Dictionary<int, GameObject>();
			_goByTag[tag] = gos;
		}
		//
		gos[go.GetInstanceID()] = go;
	}
	
	public void Register(MultiTag mt)
	{
		foreach(string tag in mt.Tags)
			Register(tag, mt.gameObject);
	}
	
	public void Unregister(string tag, GameObject go)
	{
		Dictionary<int, GameObject> gos = null;
		if (_goByTag.TryGetValue(tag,out gos))
		{
			gos.Remove(go.GetInstanceID());
		}
	}
	
	public void Unregister(MultiTag mt)
	{
		foreach(string tag in mt.Tags)
			Unregister(tag, mt.gameObject);
	}
	
	public IEnumerable<GameObject> Find(string tag)
	{
        Dictionary<int, GameObject> gos = null;
        if (!_goByTag.TryGetValue(tag, out gos))
            return Enumerable.Empty<GameObject>();

        return gos.Values;
	}
}