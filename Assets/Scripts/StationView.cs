using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StationView : MonoBehaviour {
    static int _releasedTrigger = Animator.StringToHash("released");
    Animator _animator;    

    Stationary _stationary;
    public Stationary Stationary
    {
        get { return _stationary; }
        set 
        { 
            _stationary = value;
            _stationary.OnRelease += (s) => {
                Text ammoText = transform.FindChild("Ammo").GetComponent<Text>();
                ammoText.text = _stationary.Ammo.ToString();
                if (_animator != null)
                    _animator.SetTrigger(_releasedTrigger);
            };
            Refresh();
        }
    }
    // Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
    void Refresh()
    {
        Text labelText = transform.FindChild("Label").GetComponent<Text>();
        Text ammoText = transform.FindChild("Ammo").GetComponent<Text>();
        labelText.text = _stationary.Name;
        ammoText.text = _stationary.Ammo.ToString();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
