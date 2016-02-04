using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AmmoIcon : MonoBehaviour {
    public int Slices = 4;

    int _ammo = 0;
    public int Ammo
    {
        get { return _ammo; }
        set 
        { 
            _ammo = value;
            StartCoroutine(Refresh(_ammo));
        }
    }
    Image _image;
	// Use this for initialization
	void Start ()
    {
        _image = GetComponent<Image>();
	}

    IEnumerator Refresh(int count)
    {
        yield return new WaitForEndOfFrame();
        _image.fillAmount = (float)count / Slices;
    }
}
