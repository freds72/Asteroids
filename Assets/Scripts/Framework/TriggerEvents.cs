using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class OnTriggerEnterEvent : UnityEvent<Collider>
{
}
[System.Serializable]
public class OnTriggerExitEvent : UnityEvent<Collider>
{
}
[System.Serializable]
public class OnTriggerStayEvent : UnityEvent<Collider>
{
}

public class TriggerEvents : MonoBehaviour {

    public OnTriggerEnterEvent TriggerEnter;
    public OnTriggerExitEvent TriggerExit;
    public OnTriggerStayEvent TriggerStay;

	// Use this for initialization
	void Start () { 
	}

    void OnTriggerEnter(Collider other)
    {
        if (TriggerEnter != null)
            TriggerEnter.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (TriggerExit != null)
            TriggerExit.Invoke(other);
    }

    void OnTriggerStay(Collider other)
    {
        if (TriggerStay != null)
            TriggerStay.Invoke(other);
    }
}
