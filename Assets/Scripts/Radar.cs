using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

// A radar attached to a GameObject
public class Radar : MonoBehaviour
{
    public List<RadarMode> Modes = new List<RadarMode>();
    public RadarMode EyeMode = new RadarMode() { Label = "Eye", Angle = 120, MaxRange = 5, MinRange = 0, Memory = 5.0f, ScanPeriod = 1.0f };
    public GameObject SelectionPrefab;
    public GameObject LockPrefab;

    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float TextureScale = 1.0f;

    public float SelectionSpeed = 0.35f;

    List<RadarCache> _cache = new List<RadarCache>();
    List<VectorLine> _cones = new List<VectorLine>();

    RadarSelection _selection;
    List<Transform> _locksInstanceCache = new List<Transform>(4);
    RadarCache _eyeCache;
    public int DefaultMode = 0;
    int _selectedMode = 0;
    int _lastSelectedItem = -1;
    // Use this for initialization
    void Start()
    {
        foreach (RadarMode it in Modes)
        {
            _cache.Add(new RadarCache(transform, it));

            // radar cone
            float leftAngle = Mathf.Deg2Rad * (180 - it.Angle);
            float rightAngle = Mathf.Deg2Rad * it.Angle;
            List<Vector3> points = new List<Vector3>(new Vector3[] 
            { 
                new Vector3(it.MinRange * Mathf.Cos(rightAngle), it.MinRange * Mathf.Sin(rightAngle),0),
                new Vector3(it.MaxRange * Mathf.Cos(rightAngle), it.MaxRange * Mathf.Sin(rightAngle),0),
                new Vector3(it.MinRange * Mathf.Cos(leftAngle), it.MinRange * Mathf.Sin(leftAngle),0),
                new Vector3(it.MaxRange * Mathf.Cos(leftAngle), it.MaxRange * Mathf.Sin(leftAngle),0)
            });
            VectorLine line;
            if (LineTexture == null)
                line = new VectorLine("RadarCone_" + it.Label, points, LineWidth, LineType.Discrete);
            else
                line = new VectorLine("RadarCone_" + it.Label, points, LineTexture, LineWidth, LineType.Discrete);
            line.textureScale = TextureScale;
            
            VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
            line.active = false;

            _cones.Add(line);
        }
        _eyeCache = new RadarCache(transform, EyeMode);

        // change to default selection
        ChangeMode(DefaultMode);

        _selection = Instantiate(SelectionPrefab).GetComponent<RadarSelection>();
        _selection.gameObject.SetActive(false);
    }

    void ChangeMode(int mode)
    {
        // pause scan of previous mode
        if (_selectedMode != mode)
        {
            _cache[_selectedMode].Pause();
            _cones[_selectedMode].active = false;
        }
        _lastSelectedItem = -1;
        _selectedMode = mode;
        _cones[_selectedMode].active = true;
    }

    public void NextMode()
    {
        ChangeMode(Mathf.Min(_selectedMode + 1, Modes.Count - 1));
    }

    public void PreviousMode()
    {
        ChangeMode(Mathf.Max(_selectedMode - 1, 0));
    }

    // Update is called once per frame
    float _lastFrameHValue = 0;
    void Update()
    {
        _eyeCache.Update();

        if (Modes.Count == 0)
            return; // nothing to do
        RadarCache activeRadar = _cache[_selectedMode];
        activeRadar.Update();

        // change selection
        if (Input.GetAxis("RightStickHorizontal") == 1 && _lastFrameHValue != 1)
            activeRadar.Next();
        else if (Input.GetAxis("RightStickHorizontal") == -1 && _lastFrameHValue != -1)
            activeRadar.Previous();

        if (Input.GetButtonUp("Lock") && activeRadar.SelectedItem != null)
            activeRadar.SelectedItem.IsLocked = !activeRadar.SelectedItem.IsLocked;

        _lastFrameHValue = Input.GetAxis("RightStickHorizontal");

        // update tracker positions
        RadarItem selectedItem = activeRadar.SelectedItem;
        if ( selectedItem != null )
        {
            if (_lastSelectedItem != selectedItem.InstanceID)
            {
                // move toward target
                _lastSelectedItem = selectedItem.InstanceID;
                StartCoroutine(MoveSelector(selectedItem.InstanceID,_selection.transform.position, selectedItem.Target.transform));
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
        foreach(RadarItem it in activeRadar)
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

    IEnumerator MoveSelector(int id,Vector3 sourcePosition, Transform target)
    {
        float elapsedTime = 0;
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
            _selection.TargetInfo = string.Format("{0:0.00}", Vector3.Distance(transform.position, target.position));
            yield return null;
        }
    }

    public Transform AcquireLock
    {
        get
        {
            RadarItem lockedIem = _cache[_selectedMode].AcquireLock;
            return lockedIem == null ? null : lockedIem.Target.transform;
        }
    }
}
