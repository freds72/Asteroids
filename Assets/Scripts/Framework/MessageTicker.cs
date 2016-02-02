using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game messages list. Messages can be updated by game components based on a unique ID.
/// </summary>
public class MessageTicker : MonoBehaviour {

    public GameObject TickerList;
    public GameObject TickerPrefab;
    int _id = 0;
    Dictionary<int, Text> _tickersByID = new Dictionary<int, Text>();
    Dictionary<int, GameObject> _gosByID = new Dictionary<int, GameObject>();

	// Use this for initialization
	void Start () {
        Create("hello!", 5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator ReleaseMessageTask(int id, float duration)
    {
        yield return new WaitForSeconds(duration);
        Release(id);
    }

    public int Create(string message = null)
    {
        _id++;
        GameObject go = Instantiate(TickerPrefab);
        // add to containing list
        go.transform.SetParent(TickerList.transform, false);

        Text txt = go.GetComponent<Text>();
        if (!string.IsNullOrEmpty(message))
            txt.text = message;
        _tickersByID.Add(_id, txt);
        _gosByID.Add(_id, go);

        return _id;
    }

    public void Create(string message, float duration)
    {
        StartCoroutine(ReleaseMessageTask(Create(message), duration));
    }

    public void Release(int id)
    {
        GameObject go = null;
        if (_gosByID.TryGetValue(id, out go))
        {
            Destroy(go);
            _gosByID.Remove(id);
            _tickersByID.Remove(id);
        }
    }

    public void Update(int id, string message)
    {
        Text text = null;
        if (_tickersByID.TryGetValue(id, out text))
            text.text = message;
    }
}
