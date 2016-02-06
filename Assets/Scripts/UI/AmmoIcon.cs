using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoIcon : MonoBehaviour {
    public int Slices = 4;

    int _ammo = 0;
    public int Ammo
    {
        get { return _ammo; }
        set 
        { 
            _ammo = value;
            StartCoroutine(Refresh());
        }
    }
    public Image Icon;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Refresh());
	}

    IEnumerator Refresh()
    {
        yield return new WaitForEndOfFrame();
        Refresh(_ammo);
    }

    void Refresh(int count)
    {
        Icon.fillAmount = (float)count / Slices;
    }
}
