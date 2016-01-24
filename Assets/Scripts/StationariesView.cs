using UnityEngine;
using System.Collections;

public class StationariesView : MonoBehaviour {
    public GameObject WidgetPrefab;
    public GameObject Player;

	// Use this for initialization
	void Start () {
        Stationaries stations = Player.GetComponent<Stationaries>();

        foreach (Stationary it in stations.Items)
        {
            GameObject go = Instantiate(WidgetPrefab);
            StationView sv = go.GetComponent<StationView>();
            sv.Stationary = it;
            // add to self
            sv.transform.SetParent(this.transform, false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
