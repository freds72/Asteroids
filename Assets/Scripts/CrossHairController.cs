using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TargetLink))]
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

    public TagCollection<AllTags.Values> Filter;

    Transform _parent;
    float _screenWidth;
    float _screenHeight;
    bool _precisionMode = false;
    Vector3 _move;

    void Start()
    {
        _parent = GetComponent<TargetLink>().Target;
        StartCoroutine(LowDefTracking());
    }

    void Update()
    {
        float rh = Input.GetAxis("RightStickHorizontal");
        float rv = Input.GetAxis("RightStickVertical");
        //Assumes you're looking down the z axis and that you are looking down on the avatar
        _move = new Vector3(rh, rv, 0.0f);

        if (Input.GetButtonDown("IronSight"))
        {
            _precisionMode = true;
            StartCoroutine(HiDefTracking());
        }
        else if (Input.GetButtonUp("IronSight"))
        {
            _precisionMode = false;
            StartCoroutine(LowDefTracking());
        }
    }

    IEnumerator LowDefTracking()
    {
        if (OnPrecisionChanged != null)
            OnPrecisionChanged(false);
        while (_precisionMode == false)
        {
            float len = _move.sqrMagnitude;
            if (len > DeadZone * DeadZone)
            {
                transform.position = _parent.position + Scaling * _move;
            }
            yield return null;
        }
    }

    GameObject Detect()
    {
        Vector3 pos = new Vector3(
            transform.position.x,
            transform.position.y,
            SearchDistance / 2);
        Vector3 dir = new Vector3(0, 0, -1);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir.normalized, out hit, SearchDistance))
        {
            if (hit.transform != transform)
            {
                ITagCollection tags = hit.transform.GetComponent<ITagCollection>();
                if (tags.Equals(Filter))
                {
                    return hit.transform.gameObject;
                }
            }
        }
        return null;
    }

    IEnumerator LockTarget(GameObject go)
    {
        float locktime = Time.time + LockDelay;
        // still trackibg the same object?
        while (_precisionMode == true &&
                    go == Detect())
        {
            if (Time.time >= locktime)
            {
                if (OnTargetLocked != null)
                    OnTargetLocked(go);
                break;
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

    IEnumerator HiDefTracking()
    {
        if (OnPrecisionChanged != null)
            OnPrecisionChanged(true);
        StartCoroutine(SearchTargets());
        Vector3 pos = transform.position;
        while (_precisionMode == true)
        {
            float len = _move.sqrMagnitude;
            if (len > DeadZone * DeadZone)
            {
                pos += Time.deltaTime * PrecisionScaling * _move;
                transform.position = _parent.position + pos;
            }

            yield return null;
        }
    }
}