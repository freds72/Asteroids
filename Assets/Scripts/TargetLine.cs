using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

[RequireComponent(typeof(TargetLink))]
public class TargetLine : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float LineTextureScale = 1;
    
    Transform _target;
    VectorLine _targetline;

	// Use this for initialization
	void Start () {
        TargetLink link = GetComponent<TargetLink>();
        _target = link.Target;

        if (_target != null)
        {
            List<Vector3> points = new List<Vector3>(new Vector3[] {
                transform.position,
                transform.position + transform.up
            });
            if (LineTexture == null)
                _targetline = new VectorLine(GetType().Name, points, LineWidth, LineType.Discrete, Joins.None);
            else
                _targetline = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Discrete, Joins.None);
            _targetline.textureScale = LineTextureScale;
        }
	}
	
	// Update is called once per frame
	void Update () {

        // do we still have a target?
        if (_target != null)
        {
            _targetline.points3[0] = transform.position;
            _targetline.points3[1] = _target.position;
            _targetline.Draw3D();
        }
        else if (_targetline != null)  // in case target gets destroyed
        {
            VectorLine.Destroy(ref _targetline);
            _targetline = null;
        }
	}

    void OnDestroy()
    {
        VectorLine.Destroy(ref _targetline);
    }
}
