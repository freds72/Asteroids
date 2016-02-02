using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Focusable))]
class InventoryListView : MonoBehaviour
{
    public GameObject WidgetPrefab;
    public GameObject Player;
    List<Focusable> _widgets = new List<Focusable>();
    int _focusIndex = 0;

    void Start()
	{
		Inventory inventory = Player.GetComponent<Inventory>();

		foreach(InventoryItem it in inventory.Items)
		{
            GameObject igo = Instantiate(WidgetPrefab);
            InventoryItemView iiv = igo.GetComponent<InventoryItemView>();
            iiv.Item = it;
            // 
            Focusable focus = igo.GetComponent<Focusable>();
            // disabled by default
            focus.SetFocus(false);
            // add to self
            igo.transform.SetParent(this.transform, false);
            _widgets.Add(focus);
		}
	}

    void OnEnable()
    {
        Debug.Log("Got focus");
        int i = 0;
        foreach (Focusable it in _widgets)
        {
            it.SetFocus(_focusIndex == i);
            i++;
        }        
    }

    void OnDisable()
    {
        foreach (Focusable it in _widgets)
            it.SetFocus(false);
        Debug.Log("Lost focus");
    }

    void ChangeFocus(int index)
    {
        // nothing to do
        if (index == _focusIndex)
            return;

        if (_focusIndex < _widgets.Count)
            _widgets[_focusIndex].SetFocus(false);

        _widgets[index].SetFocus(true);
        _focusIndex = index;
    }

    void Update()
    {
        if (_widgets.Count > 0 && Input.GetAxis("DPadVertical") == -1)
        {
            ChangeFocus(Mathf.Min(_focusIndex + 1, _widgets.Count - 1));
        }
        if (_widgets.Count > 0 && Input.GetAxis("DPadVertical") == 1)
        {
            ChangeFocus(Mathf.Max(_focusIndex - 1, 0));
        }
    }
}