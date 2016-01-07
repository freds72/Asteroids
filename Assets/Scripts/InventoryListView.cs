using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class InventoryListView : MonoBehaviour
{
    public GameObject WidgetPrefab;
    public GameObject Player;
    List<GameObject> _widgets = new List<GameObject>();
    int _focusIndex = 0;

    void Start()
	{
		Inventory inventory = Player.GetComponent<Inventory>();

		foreach(InventoryItem it in inventory.Items)
		{
            GameObject igo = Instantiate(WidgetPrefab);
            InventoryItemView iiv = igo.GetComponent<InventoryItemView>();
            iiv.Item = it;
			iiv.OnActivate += (e) => {
				inventory.Activate(e);
			};
            // add to self
            igo.transform.SetParent(this.transform);
            // disabled by default
            igo.SetActive(false);
            _widgets.Add(igo);
		}
	}

    void OnEnable()
    {
        int i = 0;
        foreach(GameObject it in _widgets)
        {
            it.SetActive(_focusIndex == i);
            i++;
        }
    }

    void ChangeFocus(int index)
    {
        // nothing to do
        if (index == _focusIndex)
            return;

        if (_focusIndex < _widgets.Count)
            _widgets[_focusIndex].SetActive(false);

        _widgets[index].SetActive(true);
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