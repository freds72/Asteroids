using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Weapon : MonoBehaviour
{
    public GameObject Prefab;
    public float Delay = 0.5f;
    float _nextFire = 0;
    public string InputName = "Fire";
    public bool IsSecondary = false;

    public void Fire()
    {
        Instantiate(Prefab, transform.position, transform.rotation);

        _nextFire = Time.time + Delay;
    }

    void Update()
    {
        if (Input.GetAxis(InputName) == (IsSecondary?-1:1) && Time.time > _nextFire)
            Fire();
    }
}
