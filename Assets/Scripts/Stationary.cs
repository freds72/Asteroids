using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable()]
public class Stationary {
    public delegate void ReleaseEvent(Stationary s);
    public event ReleaseEvent OnRelease;

    public enum StationaryType
    {
        Missile,
        Bomb,
        Gun
    };
    public string Name;
    public int Ammo;
    public GameObject Prefab;
    public float Delay = 0.1f;
    public StationaryType Type;
    // time in second to automatically refill ammo
    public float AutoAmmoDelay = 0;
    public List<Transform> Anchors;
    public enum ReleaseType
    {
    	None,
    	Ripple,
    	Aim
    };
    public ReleaseType ReleaseMode;
    public Transform Aim;
    
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
                t.LookAt(Aim);
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
        if (OnRelease != null)
            OnRelease(this);

        Transform transform = NextSpawnLocation();
        return (GameObject)MonoBehaviour.Instantiate(Prefab,
            transform.position,
            transform.rotation);
    }
}
