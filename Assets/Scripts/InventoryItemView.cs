using UnityEngine;
using System.Collections;
using UnityEngine.UI;

class InventoryItemView : MonoBehaviour
{
    public delegate void Activate(InventoryItem o);
	public event Activate OnActivate;
    public InventoryItem Item { get; set; }
    public Text Label;
    
	bool _activating = false;
	
    void Start()
    {
        Label.text = Item.Label;
    }

	void Update()
	{
		if (_activating == false && Input.GetAxis("DPadHorizontal")==1)
		{
			_activating = true;
			StartCoroutine("ActivationState");
		}
	}
	
	IEnumerator ActivationState()
	{
		float now = Time.time;
        while (Input.GetAxis("DPadHorizontal") == 1)
		{
			if (Time.time > now + Item.ActivationTime)
			{
				if (OnActivate != null )
                    OnActivate(Item);
				_activating = false;
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
		_activating = false;
	}
}