using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {
    /// <summary>
    /// Specifies whether to apply the rotation over the z axis (2d mode) or y axis (3d mode)
    /// </summary>
    public bool Is3D = true;
    public float Min = 0;
    public float Max = 360;
	void Awake()
    {
        float angle = Random.Range(Min, Max);
        transform.localRotation *= Quaternion.Euler(0, Is3D ? angle : 0, Is3D ? 0 : angle);
    }
}
