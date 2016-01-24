using UnityEngine;
using System.Collections;
using System;

// http://archive-server.liveatc.net/krno/KRNO-Jan-21-2016-2200Z.mp3
// http://archive-server.liveatc.net/kvgt/KVGT-Jan-21-2016-2200Z.mp3
// http://archive-server.liveatc.net/ksck/KMCO-App-Disney-Ovido-Jan-21-2016-1830Z.mp3
[RequireComponent(typeof(AudioSource))]
public class StreamATC : MonoBehaviour {

    public string URLFragment = "krno/KRNO";
    string _URLPattern = "http://archive-server.liveatc.net/{0}-{1}.mp3";

    bool _playing = false;

	// Use this for initialization
	void Start () {
	    
	}
	
    IEnumerator Stream()
    {
        string url = string.Format(_URLPattern, URLFragment, "Jan-21-2016-2200Z");
        WWW www = new WWW(url);        
        yield return www;

        if (www.error != null && www.error.Length > 0)
        {
            Debug.Log(www.error + "(" + url + ")");
        }
        else
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = www.GetAudioClip(false, false);
            while (audio.clip.loadState != AudioDataLoadState.Loaded)
                yield return null;
            audio.Play();
        }
    }

	// Update is called once per frame
	void Update () {
	    if ( !_playing )
        {
            foreach(GameObject it in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (Vector3.SqrMagnitude(it.transform.position - transform.position) < 80 * 80)
                {
                    _playing = true;
                    break;
                }
            }
            StartCoroutine(Stream());
        }
	}
}
