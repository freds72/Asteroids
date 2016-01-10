using UnityEngine;
using System.Collections;

public class RadarItem : MonoBehaviour {
    public GameObject LockedSprite;
    public GameObject SelectedSprite;
    public Transform Target;
    public float LastFiredAtTime { get; set; }
    public float LastSeenTime { get; set; }
    public bool IsLocked { get; set; }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
