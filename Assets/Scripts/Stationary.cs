using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable()]
public class Stationary {
    public enum StationaryType
    {
        Missile,
        Bomb,
        Gun
    };
    public string Name;
    public Sprite Icon;
    [Tooltip("Ammo count")]
    public int Ammo;
    public GameObject Prefab;
    [Tooltip("Delay between release")]
    public float Delay = 0.1f;
    [Tooltip("Max number of items to spawn")]
    [Range(1, 100)]
    public int Burst = 1;
    [Tooltip("Spread cone (degrees)")]
    public float Spread = 0;
    public StationaryType Type;
    [Tooltip("Time in second to automatically refill ammo")]
    public float AutoAmmoDelay = 0;
    [Tooltip("Firing anchors")]
    public List<Transform> Anchors;
    public enum ReleaseType
    {
    	None,
    	Ripple,
    	Aim
    };
    public ReleaseType ReleaseMode;
    [Tooltip("Firing direction")]
    public Transform Aim;
    [Tooltip("Release sounds")]
    public List<AudioClip> Clips;
    
    float _nextFireTime = 0;    
    public bool CanSpawn()
    {
        return Ammo > 0 && Time.time >= _nextFireTime;
    }
    
    int nextAnchor = 0;
    Transform NextSpawnLocation()
    {
        switch (ReleaseMode)
        {
            case ReleaseType.Ripple:
                nextAnchor++;
                return Anchors[nextAnchor % Anchors.Count];

            case ReleaseType.Aim:
                Transform t = Anchors
                    .OrderBy(i => Vector3.SqrMagnitude(Aim.position - i.position))
                    .First();

                Vector3 dir = Aim.position - t.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                t.rotation = Quaternion.Euler(0,0,angle - 90);
                return t;
        }
        return Anchors[0];
    }

    public virtual GameObject Spawn()
    {
#if DEBUG
        if (!CanSpawn())
            throw new InvalidProgramException("Trying to fire an item without checking CanFire");
#endif

        _nextFireTime = Time.time + Delay;
        Ammo--;
        Transform transform = NextSpawnLocation();
        if ( Spread != 0 )
            transform.rotation *= Quaternion.Euler(0,UnityEngine.Random.Range(-Spread/2.0f, Spread/2.0f),0);

        return (GameObject)MonoBehaviour.Instantiate(Prefab,
            transform.position,
            transform.rotation);
    }

    public virtual IEnumerator<GameObject> BulkSpawn()
    {
#if DEBUG
        if (!CanSpawn())
            throw new InvalidProgramException("Trying to fire an item without checking CanFire");
#endif

        _nextFireTime = Time.time + Delay;
        Ammo--;
        for (int i = 0; i < Burst; i++)
        {
            Transform transform = NextSpawnLocation();
            if (Spread != 0)
                transform.rotation *= Quaternion.Euler(0, UnityEngine.Random.Range(-Spread / 2.0f, Spread / 2.0f), 0);

            yield return (GameObject)MonoBehaviour.Instantiate(Prefab,
                transform.position,
                transform.rotation);
        }
    }
}
