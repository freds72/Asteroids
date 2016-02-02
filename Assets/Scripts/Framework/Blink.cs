using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class Blink : MonoBehaviour {

    public Material BlinkMaterial;
    public float Duration = 0.5f;
    MeshRenderer _renderer;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
        StartCoroutine(DoBlink());
	}
	
	// Update is called once per frame
	IEnumerator DoBlink () {
        Material m = _renderer.material;
        int i = 0;
        while (Duration > 0)
        {
            _renderer.material = ((i++)%2==0)?BlinkMaterial:m;
            yield return new WaitForSeconds(Duration);
        }
    }
}
