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
    // Use this for initialization
    void Start()
    {
        position = transform.position;
        _velocity = 0;
        _height = 2f * Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;
    }

    public void Scroll()
    {
        _velocity = Velocity;
    }

    public void StopScroll()
    {
        _velocity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        position += Time.deltaTime * _velocity * transform.right;
        if (Snap)
            transform.position = new Vector3(Mathf.FloorToInt(position.x) * PixelSize, position.y, position.z);
        else
            transform.position = position;
    }
}
