using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

// A radar attached to a GameObject
public class Radar : MonoBehaviour
{
    public RadarMode Specs = new RadarMode() { Label = "Radar", Angle = 120, MaxRange = 40, MinRange = 5, Memory = 1.0f, ScanPeriod = 0.5f };
    public GameObject SelectionPrefab;
    public GameObject LockPrefab;

    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float TextureScale = 1.0f;

    public float SelectionSpeed = 0.35f;

    RadarCache _radarCache;
    VectorLine _radarCone;

    RadarSelection _selection;
    List<Transform> _locksInstanceCache = new List<Transform>(4);
    RadarCache _eyeCache;
    public int DefaultMode = 0;
    int _lastSelectedItem = -1;

    // Use this for initialization
    void Start()
    {
        _radarCache = new RadarCache(transform, Specs);

        // radar cone
        float leftAngle = Mathf.Deg2Rad * (180 - Specs.Angle);
        float rightAngle = Mathf.Deg2Rad * Specs.Angle;
        List<Vector3> points = new List<Vector3>(new Vector3[] 
            { 
                new Vector3(Specs.MinRange * Mathf.Cos(rightAngle), Specs.MinRange * Mathf.Sin(rightAngle),0),
                new Vector3(Specs.MaxRange * Mathf.Cos(rightAngle), Specs.MaxRange * Mathf.Sin(rightAngle),0),
                new Vector3(Specs.MinRange * Mathf.Cos(leftAngle), Specs.MinRange * Mathf.Sin(leftAngle),0),
                new Vector3(Specs.MaxRange * Mathf.Cos(leftAngle), Specs.MaxRange * Mathf.Sin(leftAngle),0)
            });
        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine(GetType().Name, points, LineWidth, LineType.Discrete);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Discrete);
        line.textureScale = TextureScale;

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
        line.active = false;

        _radarCone = line;

        _selection = Instantiate(SelectionPrefab).GetComponent<RadarSelection>();
        _selection.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        _radarCache.Pause();
        if ( _selection != null )
            _selection.gameObject.SetActive(false);
        _lastSelectedItem = -1;
    }

    // Update is called once per frame
    float _lastFrameHValue = 0;
    void Update()
    {
        _radarCache.Update();

        // change selection
        if (Input.GetAxis("RightStickHorizontal") == 1 && _lastFrameHValue != 1)
            _radarCache.Next();
        else if (Input.GetAxis("RightStickHorizontal") == -1 && _lastFrameHValue != -1)
            _radarCache.Previous();

        if (Input.GetButtonUp("Lock") && _radarCache.SelectedItem != null)
            _radarCache.SelectedItem.IsLocked = !_radarCache.SelectedItem.IsLocked;

        _lastFrameHValue = Input.GetAxis("RightStickHorizontal");

        // update tracker positions
        RadarItem selectedItem = _radarCache.SelectedItem;
        if ( selectedItem != null )
        {
            if (_lastSelectedItem != selectedItem.InstanceID)
            {
                // move toward target
                _lastSelectedItem = selectedItem.InstanceID;
                StartCoroutine(MoveSelector(selectedItem.InstanceID,_selection.transform.position, selectedItem));
            }
            if ( !_selection.gameObject.activeInHierarchy )
                _selection.gameObject.SetActive(true);
        }
        else
        {
            if (_selection.gameObject.activeInHierarchy)
                _selection.gameObject.SetActive(false);
        }

        // update target lock position
        int i = 0;
        foreach (RadarItem it in _radarCache)
        {
            if (it.IsLocked)
            {
                // pick lock target from cache
                Transform lck = null;
                if ( i >= _locksInstanceCache.Count )
                {
                    // create a new instance
                    Transform newLock = Instantiate(LockPrefab).GetComponent<Transform>();
                    newLock.gameObject.SetActive(false);
                    _locksInstanceCache.Add(newLock);
                }                    
                lck = _locksInstanceCache[i];                
                lck.position = it.Target.transform.position;
                if (!lck.gameObject.activeInHierarchy)
                    lck.gameObject.SetActive(true);
                i++;
            }
        }
        // disable remaining locks
        for(;i<_locksInstanceCache.Count;i++)
        {
            Transform lck = _locksInstanceCache[i];
            if (lck.gameObject.activeInHierarchy)
                lck.gameObject.SetActive(false);
        }
    }

    IEnumerator MoveSelector(int id,Vector3 sourcePosition, RadarItem radarItem)
    {
        float elapsedTime = 0;
        Transform target = radarItem.Target.transform;
        Vector3 targetPosition = target.position;
        _selection.TargetInfo = "";
        while (elapsedTime < SelectionSpeed && _lastSelectedItem == id)
        {
            _selection.transform.position = Vector3.Lerp(sourcePosition, targetPosition, Mathf.SmoothStep(0, 1, (elapsedTime / SelectionSpeed)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }        
        // real-time tracking (unless we change target)        
        while (_lastSelectedItem == id)
        {
            _selection.transform.position = target.position;
            int angle = (int)Vector2.Angle(new Vector2(target.up.x, target.up.y), new Vector2(transform.up.x, transform.up.y));
            angle %= 360;
            if ( angle < 0 )
                angle += 360;
            
            _selection.TargetInfo = string.Format("{0}\u00B0\n{1:0.00}\n0.9",
                angle,
                radarItem.Distance,
                0.9f); // TODO: get target velocity
            
            yield return null;
        }
    }

    public Transform AcquireLock
    {
        get
        {
            RadarItem lockedIem = _radarCache.AcquireLock;
            return lockedIem == null ? null : lockedIem.Target.transform;
        }
    }
}
