using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable()]
public class Waypoint : MonoBehaviour {
    public string Label;
    public Text Text;
    int _id = 0;
    public int ID
    {
        get
        { return _id; }
        set
        {
            _id = value;
        }
    }

    void Start()
    {
        Invoke("RefreshText", 0.1f);
    }

    void RefreshText()
    {
        if (string.IsNullOrEmpty(Label))
            Text.text = _id.ToString();
        else
            Text.text = Label;
    }
}
