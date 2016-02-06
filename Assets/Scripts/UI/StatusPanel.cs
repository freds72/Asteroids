using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class StatusPanel : MonoBehaviour {

    int _openParam = Animator.StringToHash("open");
    int _hpParam = Animator.StringToHash("hp");
    Animator _animator;
    public Image HealthImage;
    public Text SelectedStationText;
    public AmmoPanel AmmoPanel;
    public GameObject Player;
    public AmmoIcon MissileAmmoPrefab;
    public AmmoIcon GunAmmoPrefab;

    Stationaries _stations;
    HitPoints _health;
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _stations = Player.GetComponent<Stationaries>();
        _stations.OnSelectionChanged += (s) => { Refresh(s); };
        _stations.OnRelease += (s,go) => {
            // make sure the event applies to the right station!!
            if (s.Name == SelectedStationText.text)
                AmmoPanel.UpdateAmmo(s.Ammo); 
        };
        _stations.OnRefill += (s) => {
            // make sure the event applies to the right station!!
            if (s.Name == SelectedStationText.text)
                AmmoPanel.UpdateAmmo(s.Ammo); 
        };
        Refresh(_stations.SelectedItem);
        
        // hit points
        _health = Player.GetComponent<HitPoints>();
    }

    void Refresh(Stationary s)
    {
        SelectedStationText.text = s.Name;
        switch(s.Type)
        {
            case Stationary.StationaryType.Missile:
                AmmoPanel.IconPrefab = MissileAmmoPrefab;
                break;
            case Stationary.StationaryType.Gun:
                AmmoPanel.IconPrefab = GunAmmoPrefab;
                break;

            default:
                throw new NotImplementedException("Unsupported ammo type");
        }
        
        AmmoPanel.Station = s;
    }

	// Update is called once per frame
	void Update () {
	    if ( Input.GetButtonDown("Status") )
            _animator.SetBool(_openParam, true);

        if (Input.GetButtonUp("Status"))
            _animator.SetBool(_openParam, false);

        // health is temperature, reverse
        float hpRatio = 1.0f - (float)_health.HP / _health.MaxHP;

        HealthImage.fillAmount = 0.5f * hpRatio; // scale is [0;0.5]
        _animator.SetFloat(_hpParam, hpRatio);
	}
}
