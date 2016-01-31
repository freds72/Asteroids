using UnityEngine;

public class OnOffSwitch : MonoBehaviour
{
	public virtual bool IsOn
	{
		get { return gameObject.activeInHierarchy; }
	}

	public virtual void OnOff(bool state)
	{
		gameObject.SetActive(state);
	}
}