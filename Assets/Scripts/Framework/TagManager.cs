using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TagManager
{
	public static readonly TagManager Self = new TagManager();
	
	Dictionary<long, Dictionary<int, GameObject>> _goByTag = new Dictionary<long, Dictionary<int, GameObject>>();
	
	void Register(long tag, int id, GameObject go)
	{
		Dictionary<int, GameObject> gos = null;
		if (!_goByTag.TryGetValue(tag,out gos))
		{
			gos = new Dictionary<int, GameObject>();
			_goByTag[tag] = gos;
		}
		//
		gos[id] = go;
	}
	
	public void Register(ITagCollection mt)
	{
        int id = mt.GameObject.GetInstanceID();
        for (int i = 0; i < 64; ++i)
        {
            long tag = mt.Mask & (1L << i);
            if ( tag != 0L )
                Register(tag, id, mt.GameObject);
        }
	}
	
	void Unregister(long tag, int id)
	{
		Dictionary<int, GameObject> gos = null;
		if (_goByTag.TryGetValue(tag,out gos))
		{
			gos.Remove(id);
		}
	}

    public void Unregister(ITagCollection mt)
    {
        int id = mt.GameObject.GetInstanceID();
        for (int i = 0; i < 64; ++i)
        {
            long tag = mt.Mask & (1L << i);
            if (tag != 0L)
                Unregister(tag, id);
        }
    }
	
	public IEnumerable<GameObject> Find<T>(T tag) where T: struct
	{
        Dictionary<int, GameObject> gos = null;
        if (!_goByTag.TryGetValue(Convert.ToInt64(tag), out gos))
            return Enumerable.Empty<GameObject>();

        return gos.Values;
	}
}