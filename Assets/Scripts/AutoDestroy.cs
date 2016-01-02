using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

    public float Duration = 1.0f;
    // Gameobject to spawn when this one dies
    public GameObject Prefab;
    float _startTime = 0;

    /// <summary>
    /// Returns remaining object lifetime
    /// </summary>
    public float Lifetime
    {
        get { return Time.time - _startTime; }
    }

    void Start()
    {
        _startTime = Time.time;
        Invoke("DoDestroy", Duration);
    }

    void DoDestroy()
    {
        Destroy(gameObject);
        if (Prefab != null)
            Instantiate(Prefab, transform.position, transform.rotation);
    }
}
