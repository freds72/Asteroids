using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TagManager
{
    static Dictionary<long, Dictionary<int, GameObject>> _goByTag = new Dictionary<long, Dictionary<int, GameObject>>();

    static void Register(long tag, GameObject go)
    {
        Dictionary<int, GameObject> gos = null;
        if (!_goByTag.TryGetValue(tag, out gos))
        {
            gos = new Dictionary<int, GameObject>();
            _goByTag[tag] = gos;
        }
        //
        gos[go.GetInstanceID()] = go;
    }

    public static void Register(ITagCollection tags, GameObject go)
    {
        for (int i = 0; i < 64; ++i)
        {
            long tag = tags.Mask & (1L << i);
            if (tag != 0L)
                Register(tag, go);
        }
    }

    static void Unregister(long tag, int id)
    {
        Dictionary<int, GameObject> gos = null;
        if (_goByTag.TryGetValue(tag, out gos))
        {
            gos.Remove(id);
        }
    }

    public static void Unregister(ITagCollection tags, GameObject go)
    {
        for (int i = 0; i < 64; ++i)
        {
            long tag = tags.Mask & (1L << i);
            if (tag != 0L)
                Unregister(tag, go.GetInstanceID());
        }
    }

    public static IEnumerable<GameObject> Find<T>(T tag) where T : struct
    {
        Dictionary<int, GameObject> gos = null;
        if (_goByTag.TryGetValue(Convert.ToInt64(tag), out gos))
        {
            return gos.Values;
        }
        return Enumerable.Empty<GameObject>();
    }

    public static IEnumerable<GameObject> FindAny<T>(T tag)
    {
        List<IEnumerable<GameObject>> all = new List<IEnumerable<GameObject>>(4);
        long mask = Convert.ToInt64(tag);
        for (int i = 0; i < 64; i++)
        {
            if ((mask & (1L << i)) == 0)
                continue;
            Dictionary<int, GameObject> gos = null;
            if (_goByTag.TryGetValue(Convert.ToInt64(tag), out gos))
            {
                all.Add(gos.Values);                     
            }
        }
        // virtual merge of all collections
        return all.SelectMany(x => x);
    }
}