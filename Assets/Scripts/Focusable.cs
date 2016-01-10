using UnityEngine;
using System.Collections;

/// <summary>
/// Focus behavior. Mostly for UI elements.
/// </summary>
/// <remarks>
/// An optional animator component can be attached for focus visual feedback. Animation parameter is: focus (boolean).</remarks>
public class Focusable : MonoBehaviour {

    static int _focusParam = Animator.StringToHash("focus");
    Animator _animator;    

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
            _animator.SetBool(_focusParam, enabled);
    }

    public void SetFocus(bool focus)
    {
        if (enabled != focus)
        {
            enabled = focus;
            if (_animator != null)
                _animator.SetBool(_focusParam, enabled);
        }
    }

    public bool HasFocus
    { 
        get { return enabled; } 
    }
}
