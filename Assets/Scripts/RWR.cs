using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class RWR : MonoBehaviour
{
	public float RadarDetectionRange = 30;
	public float LaunchDetectionRange = 10;
    public float MinRange = 0.5f;
	public float ScanDelay = 0.01f;
	public AllTags.Values RadarTag = AllTags.Values.Emitter;
    public AllTags.Values LaunchTag = AllTags.Values.Launch;
    public OnOffSwitch RadarWarningPrefab;
    public OnOffSwitch LaunchWarningPrefab;
	// number of display sections
	public int Precision = 16;
	public float Radius = 1;
    public AudioClip RadarWarningSound;
    public AudioClip LaunchAlertSound;

    List<OnOffSwitch> _warnings = new List<OnOffSwitch>();
    List<OnOffSwitch> _alerts = new List<OnOffSwitch>();

    AudioSource _audio;

	void Start()
	{
        Vector3 offset = Radius * Vector3.up;        
		for(int i=0;i<Precision;i++)
		{
            // rotate clockwise
            Quaternion angle = Quaternion.Euler(0, 0, -360f * i / (Precision-1));

            OnOffSwitch warningIcon = Instantiate(RadarWarningPrefab, angle * offset, angle) as OnOffSwitch;
            warningIcon.gameObject.transform.SetParent(gameObject.transform, false);
            warningIcon.OnOff(false);

            _warnings.Add(warningIcon);

            OnOffSwitch alertIcon = Instantiate(LaunchWarningPrefab, angle * offset, angle) as OnOffSwitch;
            alertIcon.gameObject.transform.SetParent(gameObject.transform, false);
            alertIcon.OnOff(false);

            _alerts.Add(alertIcon);
		}

        _audio = GetComponent<AudioSource>();

        StartCoroutine(Scan());
	}
	
	void Update()
	{
	}

    bool Scan(AllTags.Values tag, bool[] states)
    {
        bool active = false;
        Array.Clear(states, 0, states.Length);
        foreach (GameObject it in TagManager.Find(tag))
        {
            Vector3 diff = it.transform.position - transform.position;
            // too close?
            if (diff.sqrMagnitude < MinRange * MinRange)
                continue;

            // TODO: simplify with a single atan
            int angle = (int)Vector2.Angle(new Vector2(transform.up.x, transform.up.y), new Vector2(diff.x, diff.y));
            // orientation?
            Vector3 cross = Vector3.Cross(transform.up, diff);
            if (cross.z > 0)
                angle = 360 - angle;

            // map angle to warning lights
            int i = (angle * (Precision-1)) / 360;

            states[i] = true;
            active = true;
        }
        return active;
    }

    IEnumerator Scan()
    {
        yield return new WaitForEndOfFrame();

        bool[] warns = new bool[Precision];
        bool[] launches = new bool[Precision];
        
        while (true)
        {
            Scan(RadarTag, warns);
            Scan(LaunchTag, launches);
            
            for (int i = 0; i < Precision; i++)
            {
                _warnings[i].OnOff(warns[i]);
                _alerts[i].OnOff(launches[i]);
            }

            yield return new WaitForSeconds(ScanDelay);
        }
    }
}