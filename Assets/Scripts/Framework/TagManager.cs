using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TagManager
{
    static Dictionary<long, Dictionary<int, GameObject>> _goByBits = new Dictionary<long, Dictionary<int, GameObject>>();
    static Dictionary<long, Dictionary<int, GameObject>> _goByTags = new Dictionary<long, Dictionary<int, GameObject>>();

    static void Register(Dictionary<long, Dictionary<int, GameObject>> dic, long tag, GameObject go)
    {
        Dictionary<int, GameObject> gos = null;
        if (!dic.TryGetValue(tag, out gos))
        {
            gos = new Dictionary<int, GameObject>();
            dic[tag] = gos;
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
                Register(_goByBits, tag, go);
        }
        Register(_goByTags, tags.Mask, go);
    }

    static void Unregister(Dictionary<long, Dictionary<int, GameObject>>dic, long tag, int id)
    {
        Dictionary<int, GameObject> gos = null;
        if (dic.TryGetValue(tag, out gos))
        {
            gos.Remove(id);
        }
    }

    public static void Unregister(ITagCollection tags, GameObject go)
    {
        int id = go.GetInstanceID();
        for (int i = 0; i < 64; ++i)
        {
            long tag = tags.Mask & (1L << i);
            if (tag != 0L)
                Unregister(_goByBits, tag, id);
        }
        Unregister(_goByTags, tags.Mask, id);
    }

    /// <summary>
    /// Find the set of object exactly matching the given tags
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static IEnumerable<GameObject> Find(ITagCollection tags)
    {        
        Dictionary<int, GameObject> gos = null;
        if (_goByTags.TryGetValue(tags.Mask, out gos))
            return gos.Values;
        return Enumerable.Empty<GameObject>();
    }

    public static IEnumerable<GameObject> Find<C>(IEnumerable<C> tags) where C : struct
    {
        Dictionary<int, GameObject> gos = null;
        long mask = 0L;
        foreach (C it in tags)
            mask |= Convert.ToInt64(it);
        if (_goByTags.TryGetValue(mask, out gos))
            return gos.Values;
        return Enumerable.Empty<GameObject>();
    }

    /// <summary>
    /// Returns all gameobject matching at least one of the provided tags
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static IEnumerable<GameObject> FindAny<C>(IEnumerable<C> tags) where C : struct
    {
        List<IEnumerable<GameObject>> all = new List<IEnumerable<GameObject>>(4);
        foreach (C it in tags)
        {
            // incoming collection is already split into bits
            long mask = Convert.ToInt64(it);
            Dictionary<int, GameObject> gos = null;
            if (_goByBits.TryGetValue(mask, out gos))
                all.Add(gos.Values);
        }
        // virtual merge of all collections
        return all.SelectMany(x => x);
    }

    public static IEnumerable<GameObject> FindAny(ITagCollection tags)
    {
        List<IEnumerable<GameObject>> all = new List<IEnumerable<GameObject>>(4);
        long mask = tags.Mask;
        for (int i = 0; i < 64; i++)
        {
            // extract the single bit
            if ((mask & (1L << i)) == 0)
                continue;
            Dictionary<int, GameObject> gos = null;
            if (_goByBits.TryGetValue(mask, out gos))
            {
                all.Add(gos.Values);                     
            }
        }
        // virtual merge of all collections
        return all.SelectMany(x => x);
    }

    public static IEnumerable<GameObject> FindAny(long mask)
    {
        List<IEnumerable<GameObject>> all = new List<IEnumerable<GameObject>>(4);
        for (int i = 0; i < 64; i++)
        {
            // extract the single bit
            if ((mask & (1L << i)) == 0)
                continue;
            Dictionary<int, GameObject> gos = null;
            if (_goByBits.TryGetValue(mask, out gos))
            {
                all.Add(gos.Values);
            }
        }
        // virtual merge of all collections
        return all.SelectMany(x => x);
    }

    public static GameObject Closest(this IEnumerable<GameObject> data, Vector3 position)
    {
        float dist = float.MaxValue;
        GameObject match = null;
        foreach(GameObject it in data)
        {
            float currDist = Vector3.SqrMagnitude(position - it.transform.position);
            if ( currDist < dist )
            {
                dist = currDist;
                match = it;
            }
        }

        return match;
    }
}