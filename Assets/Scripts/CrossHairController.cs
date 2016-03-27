using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TargetLink))]
[RequireComponent(typeof(Animator))]
public class CrossHairController : MonoBehaviour,
    IRadarController
{
    public delegate void PrecisionChanged(bool hidef);
    public event PrecisionChanged OnPrecisionChanged;

    public event TargetLocked OnTargetLocked;
    public float DeadZone = 0.15f;
    public Vector3 Aim
    { get { return transform.position; } }
    public float Scaling = 4;
    public float PrecisionScaling = 0.5f;
    public float SearchDistance = 2;
    public float LockDelay = 0.5f;
    public float ScanDelay = 0.1f;
    
    public List<AllTags.Values> Filter;

    Transform _parent;
    float _screenWidth;
    float _screenHeight;
    bool _precisionMode = false;
    Vector3 _move;
    Animator _anim;

    static int _lockParam = Animator.StringToHash("lock");

    void Start()
    {
        _parent = GetComponent<TargetLink>().Target;
        _anim = GetComponent<Animator>();
        StartCoroutine(LowDefTracking());
    }

    void Update()
    {
        float rh = Input.GetAxis("RightStickHorizontal");
        float rv = Input.GetAxis("RightStickVertical");
        //Assumes you're looking down the z axis and that you are looking down on the avatar
        _move = new Vector3(rh, rv, 0.0f);
        float len = _move.sqrMagnitude;
        if (len < DeadZone * DeadZone)
            _move = _parent.rotation * (Scaling * Vector3.up);
        else
            _move = Vector3.ClampMagnitude(_move, Scaling);

        if (Input.GetButtonDown("IronSight"))
        {
            _precisionMode = true;
            _anim.SetBool(_lockParam, _precisionMode);
            StartCoroutine(HiDefTracking(_move));
        }
        else if (Input.GetButtonUp("IronSight"))
        {
            _precisionMode = false;
            _anim.SetBool(_lockParam, _precisionMode);
            StartCoroutine(LowDefTracking());
        }
    }

    void LateUpdate()
    {

    }

    IEnumerator LowDefTracking()
    {
        if (OnPrecisionChanged != null)
            OnPrecisionChanged(false);
        while (_precisionMode == false)
        {
            transform.position = _parent.position + _move;

            yield return null;
        }
    }

    GameObject Detect()
    {
        Vector3 pos = new Vector3(
            transform.position.x,
            transform.position.y,
            -5f);
        RaycastHit hit;
        Debug.DrawLine(pos, pos + new Vector3(0, 0, 5));
        if (Physics.Raycast(pos, new Vector3(0,0,5), out hit, 10) &&
            hit.transform != transform)
        {
            Debug.Log("Hit something !");
            ITagCollection tags = hit.transform.GetComponent<ITagCollection>();
            if (tags != null && tags.Intersects(Filter))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    IEnumerator LockTarget(GameObject go)
    {
        float locktime = Time.time + LockDelay;
        // still tracking the same object?
        while (_precisionMode == true && go == Detect())
        {
            if (Time.time >= locktime)
            {
                if (OnTargetLocked != null)
                    OnTargetLocked(go);
                yield break;
            }
            yield return new WaitForSeconds(LockDelay);
        }
    }

    IEnumerator SearchTargets()
    {
        while (_precisionMode == true)
        {
            GameObject go = Detect();
            if (go != null)
                yield return LockTarget(go);
            else
                yield return new WaitForSeconds(ScanDelay);
        }
    }

    IEnumerator HiDefTracking(Vector3 pos)
    {
        if (OnPrecisionChanged != null)
            OnPrecisionChanged(true);
        StartCoroutine(SearchTargets());
        while (_precisionMode == true)
        {
            pos += Time.deltaTime * PrecisionScaling * _move;
            transform.position = _parent.position + pos;

            yield return null;
        }
    }
}