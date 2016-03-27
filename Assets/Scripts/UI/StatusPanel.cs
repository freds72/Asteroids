using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class StatusPanel : MonoBehaviour {

    int _openParam = Animator.StringToHash("open");
    Animator _animator;
    public Text HealthText;
    public Image WeaponImage;
    public Text AmmoText;
    public GameObject Player;

    Stationaries _stations;
    HitPoints _health;
    string _selectedWeapon = null;
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _stations = Player.GetComponent<Stationaries>();
        _stations.OnSelectionChanged += (s) => { 
            Refresh(s); 
        };
        _stations.OnRelease += (s,go) => {
            // make sure the event applies to the right station!!
            if (s.Name == _selectedWeapon)
                AmmoText.text = s.Ammo.ToString();              
        };
        _stations.OnRefill += (s) => {
            // make sure the event applies to the right station!!
            if (s.Name == _selectedWeapon)
                AmmoText.text = s.Ammo.ToString();   
        };
        Refresh(_stations.SelectedItem);
        
        // hit points
        _health = Player.GetComponent<HitPoints>();
        HealthText.text = _health.HP.ToString();

        _health.OnHPChanged += (go, hp) => {
            HealthText.text = hp.ToString();
        };
    }

    void Refresh(Stationary s)
    {
        _selectedWeapon = s.Name;
        AmmoText.text = s.Ammo.ToString();
        // TODO: image!
    }

	// Update is called once per frame
	void Update () {
	    if ( Input.GetButtonDown("Status") )
            _animator.SetBool(_openParam, true);

        if (Input.GetButtonUp("Status"))
            _animator.SetBool(_openParam, false);
	}
}
