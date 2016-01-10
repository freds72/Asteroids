using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Inventory : MonoBehaviour {

    public List<InventoryItem> Items = new List<InventoryItem>();
    
	public GameObject ItemSpawnPrefab;
	
	// Use this for initialization
	void Start () {
		foreach(InventoryItem it in Items)
		{
			it.OnActivate += (i) => {
				GameObject go = Instantiate(ItemSpawnPrefab, transform.position, Quaternion.identity) as GameObject;
				ItemSpawn spawn = go.GetComponent<ItemSpawn>();
				spawn.Item = i;
			};
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
