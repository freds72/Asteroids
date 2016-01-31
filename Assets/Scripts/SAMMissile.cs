using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

[RequireComponent(typeof(TargetLink))]
public class SAMMissile : MonoBehaviour {
    public float Velocity = 0.7f;
    public float BlastRadius = 0.5f; // kms
    public float Damage = 10;
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float Size = 1;
    public float HomingSensitivity = 0.1f;
    public float MaxHomingSensitivity = 5f;
    public GameObject BlastFlashPrefab;

    Transform _target;

	// Use this for initialization
	void Start () {
        float hsize = Size / 2;
        List<Vector3> points = new List<Vector3>(new Vector3[] {
            new Vector3(0,hsize,0),
            new Vector3(0,-hsize,0),
            new Vector3(-0.25f * hsize,0.7f * hsize,0),
            new Vector3(0.25f * hsize,0.7f * hsize,0),
            new Vector3(-0.25f * hsize,-0.8f * hsize,0),
            new Vector3(0.25f * hsize,-0.8f * hsize,0),
        });
        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine(GetType().Name, points, LineWidth, LineType.Discrete, Joins.None);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Discrete, Joins.None);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);

        // any TargetLine we need to take care of?
        TargetLink link = GetComponent<TargetLink>();
        if ( link != null )
            _target = link.Target;

        // 
        if ( BlastFlashPrefab != null )
            Instantiate(BlastFlashPrefab, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
    float _g,_lastg;
    void Update()
    {
        if (_target != null)
        {
            Vector3 diff = transform.position - _target.position;
            if ( diff.sqrMagnitude < BlastRadius * BlastRadius )
            {
                Invoke("Boom", 0.2f);
            }
            Quaternion rotation = Quaternion.LookRotation(diff, Vector3.forward);
            rotation.x = 0;
            rotation.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _g);
            // TODO: improve!!
            if (_g < MaxHomingSensitivity)
            {
                _lastg += Time.deltaTime * Time.deltaTime * HomingSensitivity;
                _g += _lastg;
            }
        }
        transform.position += Time.deltaTime * Velocity * transform.up;
    }

    void Boom()
    {
        SendMessage("DoDestroy", SendMessageOptions.DontRequireReceiver); // trigger any "destroy" logic 
        Destroy(gameObject);
    }
}
