using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class AnimatedOnOffSwitch : OnOffSwitch
{
    public bool On = true;
    static int _param = Animator.StringToHash("onoff");
    Animator _animator;

    public override bool IsOn
    {
        get { return On; }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool(_param, IsOn);
    }

    public override void OnOff(bool state)
    {
        if (state != On)
        {
            On = state;
            _animator.SetBool(_param, IsOn);
        }
    }
}