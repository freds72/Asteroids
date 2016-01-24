using UnityEngine;
using System.Collections;
using System;

[Serializable()]
public class Stationary {
    public delegate void ReleaseEvent(Stationary s);
    public event ReleaseEvent OnRelease;

    public enum StationaryType
    {
        Missile,
        Bomb
    };
    public string Name;
    public int Ammo;
    public GameObject Prefab;
    public float Delay = 0.1f;
    public StationaryType Type;

    float _nextFireTime = 0;    
    public bool CanSpawn()
    {
        return Ammo > 0 && Time.time >= _nextFireTime;
    }

    public GameObject Spawn(Transform transform)
    {
#if DEBUG
        if (!CanSpawn())
            throw new InvalidProgramException("Trying to fire an item without checking CanFire");
#endif

        _nextFireTime = Time.time + Delay;
        Ammo--;
        if (OnRelease != null)
            OnRelease(this);
        return (GameObject)MonoBehaviour.Instantiate(Prefab, transform.position, transform.rotation);    
    }
}
