using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Focusable))]
class InventoryItemView : MonoBehaviour
{
    InventoryItem _item;
    Focusable _focus;
    public InventoryItem Item 
    {
        get { return _item; }
        set
        {
            _item = value;
            Refresh();
        }
    }

    public Text Label;
    public Image ProgressBar;
    
	bool _activating = false;
    
    void Start()
    {
        _focus = GetComponent<Focusable>();
    }

    void Refresh()
    {
        if ( Item != null )
            Label.text = Item.Label;
    }

	void Update()
	{
		if (_activating == false &&
			Item != null && 
            Item.CanActivate &&
            _focus.HasFocus &&
			Input.GetAxis("DPadHorizontal")==1)
		{
			_activating = true;
			StartCoroutine(ActivationState());
		}
	}
	
	IEnumerator ActivationState()
	{
		float now = Time.time;
        while (_focus.HasFocus && Input.GetAxis("DPadHorizontal") == 1)
		{
            float progress = (Time.time - now) / Item.ActivationTime;
            ProgressBar.fillAmount = progress;

			if (Time.time > now + Item.ActivationTime)
			{
                // wait for button to be released
                float blinkTime = Time.time;
                while (_focus.HasFocus && Input.GetAxis("DPadHorizontal") != 0)
                {
                    if ( Time.time > blinkTime + 0.1f )
                    {
                        blinkTime = Time.time;
                        ProgressBar.enabled = !ProgressBar.enabled;
                    }
                    yield return new WaitForEndOfFrame();
                }
				_activating = false;
                ProgressBar.fillAmount = 0;
                // now really activate the item
                Item.Activate();
                yield break;
			}
			yield return new WaitForEndOfFrame();
		}
		_activating = false;
        ProgressBar.fillAmount = 0;
	}
}