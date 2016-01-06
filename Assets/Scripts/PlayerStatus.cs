using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

    public Ship Player;
    public Text PlayerNameText;
	// Use this for initialization
	void Start () {
        PlayerNameText.text = Player.Name;               
	}
	
	// Update is called once per frame
	void Update () {	
	}
}
