using UnityEngine;
using System.Collections;

public class StatusPanel : MonoBehaviour {

    int _openParam = Animator.StringToHash("open");
    Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if ( Input.GetButtonDown("Status") )
        {
            _animator.SetBool(_openParam, true);
        }
        if (Input.GetButtonUp("Status"))
        {
            _animator.SetBool(_openParam, false);
        }
	}
}
