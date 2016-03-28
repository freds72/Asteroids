using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Orthographic camera infinite scrolling
/// </summary>
public class ConstantCameraTranslation : MonoBehaviour
{
    /// <summary>
    /// Velocity
    /// </summary>
    public float Velocity = 10;
    public bool Snap = false;
    public float PixelSize = 1.0f / 16f;

    Vector3 position;
    float _height;
    float _width;
    float _velocity;

    public float Distance { get; set; }

    // Use this for initialization
    void Start()
    {
        position = transform.position;
        _velocity = 0;
        _height = 2f * Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;
        Distance = 0;
        StartCoroutine(Log());
    }

    public void Scroll()
    {
        _velocity = Velocity;
    }

    public void StopScroll()
    {
        _velocity = 0;
    }

    IEnumerator Log()
    {
        Debug.Log("Distance: " + Distance);
        yield return new WaitForSeconds(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = Time.deltaTime * _velocity * transform.right;
        position += delta;
        // keep track of the distance
        Distance += delta.x;

        if (Snap)
            transform.position = new Vector3(Mathf.FloorToInt(position.x) * PixelSize, position.y, position.z);
        else
            transform.position = position;
    }
}
