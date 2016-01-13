using UnityEngine;
using System.Collections;

public class RadarSignature : MonoBehaviour {

    /// <summary>
    /// Radar icon (when spotted
    /// </summary>
    public GameObject VisibleAvatar;
    /// <summary>
    /// Radar icon (when not spotted)
    /// </summary>
    /// <remarks>
    /// Only used for debug
    /// </remarks>
    public GameObject HiddenAvatar;

    int _count = 0;
    public int Spotted
    {
        get { return _count; }
        set
        {
            _count = Mathf.Max(0,value);
            Refresh();
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
    void Refresh()
    {
        // switch visibility as needed
        if (_count == 0 && VisibleAvatar.activeInHierarchy == true)
        {
            VisibleAvatar.SetActive(false);
#if DEBUG
            HiddenAvatar.SetActive(true);
#endif
        }
        else if (_count > 0 && VisibleAvatar.activeInHierarchy == false)
        {
            VisibleAvatar.SetActive(true);
#if DEBUG
            HiddenAvatar.SetActive(false);
#endif
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
