using UnityEngine;
using System;

[Serializable()]
class InventoryItem
{
	public Sprite Icon;
	public string Label;
	public int Count;
	public float ActivationTime = 1.0f;
	public GameObject Me;
}
