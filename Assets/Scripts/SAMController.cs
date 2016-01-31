using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(NPCRadar))]
public class SAMController : MonoBehaviour
{
	public int Ammo = 10;
	public GameObject SAMPrefab;
	public float FireDelay = 1;
	public float ShootAgainDelay = 30;
	// reduce radar range to get better missile accuracy
	public float RadarRangeRatio = 0.8f;
    public float RotationVelocity = 12; // degrees/s

	Dictionary<int, float> _timerByInstanceID = new Dictionary<int, float>();
    NPCRadar _radar;

	void Start() {
		_radar = GetComponent<NPCRadar>();

        StartCoroutine(Scan());
	}

	bool CanFire(int id)
	{
		float timer = 0;
        _timerByInstanceID.TryGetValue(id, out timer);
		return Time.time > timer;
	}
	
	IEnumerator Scan()
	{
        // make sure init is done
        yield return new WaitForEndOfFrame();

		float effectiveRange = RadarRangeRatio * _radar.Specs.MaxRange;
		while(Ammo > 0)
		{
            // get radar targets
            // check if already fired at
            // check if sam should have hit
            // fire!
			foreach(RadarItem it in _radar)
			{
				if ( it.Distance < effectiveRange && CanFire(it.InstanceID) )
				{
                    Ammo--;
                    _timerByInstanceID[it.InstanceID] = Time.time + ShootAgainDelay; // next firing option
                    TargetLink tl = ((GameObject)Instantiate(SAMPrefab, transform.position, Quaternion.identity)).GetComponent<TargetLink>();
                    tl.Target = it.Target.transform;

#if DEBUG
                    TargetLink mylink = GetComponent<TargetLink>();
                    if (mylink != null)
                        mylink.Target = it.Target.transform;
#endif
					break;
				}
			}
			yield return new WaitForSeconds(FireDelay);
		}
	}
	
	void Update() {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * RotationVelocity);
	}
}