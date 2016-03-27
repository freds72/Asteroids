using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MultiTag))]
public class Player : 
    MonoBehaviour,
    IPlayerIndex
{
    public string Name;

	// Update is called once per frame
	void Update () {
    }

    Enums.PlayerIndex _idx = Enums.PlayerIndex.One;
    
    public Enums.PlayerIndex PlayerIndex
    {
        get { return _idx;  }
        set { _idx = value; }
    }
}
