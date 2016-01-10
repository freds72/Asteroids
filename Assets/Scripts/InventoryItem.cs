using UnityEngine;
using System;

[Serializable()]
class InventoryItem
{
	public delegate void ActivateEvent(InventoryItem o);
	public event ActivateEvent OnActivate;
	public Sprite Icon;
	public string Label;
	public int Count;
	public bool Infinite = true;
	public float ActivationTime = 1.0f;
	public float SpawnTime = 3.0f;
	public GameObject Prefab;
	
	public bool CanActivate
	{
		get { return Infinite || Count > 0; }
	}
	
	public void Activate()
	{
        if (OnActivate != null)
        {
            OnActivate(this);
            if (!Infinite)
                Count = Math.Max(Count - 1, 0);
        }
	} 
}
