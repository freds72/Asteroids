using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatusPanel : MonoBehaviour {

    int _openParam = Animator.StringToHash("open");
    Animator _animator;
    public Image HealthImage;
    public Text SelectedStationText;
    public AmmoPanel AmmoPanel;
    public GameObject Player;
    public AmmoIcon MissileAmmoPrefab;
    public AmmoIcon GunAmmoPrefab;

    Stationaries _stations;
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _stations = Player.GetComponent<Stationaries>();
        _stations.OnSelectionChanged += _stations_OnSelectionChanged;

        Refresh(_stations.SelectedItem);
    }

    void _stations_OnSelectionChanged(Stationary s)
    {
        Refresh(s);
    }
	
    void Refresh(Stationary s)
    {
        SelectedStationText.text = s.Name;
        AmmoPanel.IconPrefab = MissileAmmoPrefab;
        AmmoPanel.Station = s;
    }

	// Update is called once per frame
	void Update () {
	    if ( Input.GetButtonDown("Status") )
            _animator.SetBool(_openParam, true);

        if (Input.GetButtonUp("Status"))
            _animator.SetBool(_openParam, false);

        HealthImage.fillAmount = 0.5f * Mathf.Abs(Mathf.Sin(2 * Time.time));
	}
}
