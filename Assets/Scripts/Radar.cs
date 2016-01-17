using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

// A radar attached to a GameObject
public class Radar : MonoBehaviour,IEnumerable<RadarItem>
{
    public List<RadarMode> Modes = new List<RadarMode>();
    public RadarMode EyeMode = new RadarMode() { Label = "Eye", Angle = 120, MaxRange = 5, MinRange = 0, Memory = 5.0f, ScanPeriod = 1.0f };
    public GameObject SelectionPrefab;
    public GameObject LockPrefab;

    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float TextureScale = 1.0f;

    List<RadarCache> _cache = new List<RadarCache>();
    List<VectorLine> _cones = new List<VectorLine>();

    Transform _selection;
    List<Transform> _locks = new List<Transform>(4);
    RadarCache _eyeCache;
    public int DefaultMode = 0;
    int _selectedMode = 0;
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

        _selection = Instantiate(SelectionPrefab).GetComponent<Transform>();
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
            _selection.position = selectedItem.Target.transform.position;
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
                if ( i >= _locks.Count )
                {
                    // create a new instance
                    Transform newLock = Instantiate(LockPrefab).GetComponent<Transform>();
                    newLock.gameObject.SetActive(false);
                    _locks.Add(newLock);
                }                    
                lck = _locks[i];                
                lck.position = it.Target.transform.position;
                if (!lck.gameObject.activeInHierarchy)
                    lck.gameObject.SetActive(true);
                i++;
            }
        }
        // disable remaining locks
        for(;i<_locks.Count;i++)
        {
            Transform lck = _locks[i];
            if (lck.gameObject.activeInHierarchy)
                lck.gameObject.SetActive(false);
        }
    }

    public IEnumerator<RadarItem> GetEnumerator()
    {
        if (_cache.Count == 0)
           return Enumerable.Empty<RadarItem>().GetEnumerator();
        return _cache[_selectedMode].GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        if (_cache.Count == 0)
            return Enumerable.Empty<RadarItem>().GetEnumerator();
        return _cache[_selectedMode].GetEnumerator();
    }
}
