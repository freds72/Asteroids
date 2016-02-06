using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

[RequireComponent(typeof(NPCRadar))]
public class SAMController : MonoBehaviour
{
    public float LineWidth = 1;
    public Texture LineTexture = null;
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
        List<Vector3> points = new List<Vector3>(new Vector3[] {
            new Vector3(0.117f, 0.18f, -1.533f), new Vector3(-0.353f, 1.093f, -1.109f), new Vector3(-0.353f, 1.093f, -1.109f), new Vector3(0.834f, 0.569f, -1.306f), new Vector3(0.834f, 0.569f, -1.306f), new Vector3(0.117f, 0.18f, -1.533f), new Vector3(0.117f, 0.18f, -1.533f), new Vector3(-0.154f, -0.791f, -1.501f), new Vector3(-0.154f, -0.791f, -1.501f), new Vector3(-0.419f, -0.056f, -1.633f), new Vector3(-0.419f, -0.056f, -1.633f), new Vector3(0.117f, 0.18f, -1.533f), new Vector3(1.094f, -0.488f, -1.247f), new Vector3(-0.154f, -0.791f, -1.501f), new Vector3(0.117f, 0.18f, -1.533f), new Vector3(1.094f, -0.488f, -1.247f), new Vector3(-0.704f, -0.648f, 0.533f), new Vector3(-0.713f, -0.3f, 1.24f), new Vector3(-0.713f, -0.3f, 1.24f), new Vector3(-1.074f, -0.269f, -0.467f), new Vector3(-1.074f, -0.269f, -0.467f), new Vector3(-0.704f, -0.648f, 0.533f), new Vector3(-0.479f, -1.042f, -0.483f), new Vector3(-0.704f, -0.648f, 0.533f), new Vector3(-1.074f, -0.269f, -0.467f), new Vector3(-0.479f, -1.042f, -0.483f), new Vector3(-0.479f, -1.042f, -0.483f), new Vector3(-0.188f, -1.377f, 0.273f), new Vector3(-0.188f, -1.377f, 0.273f), new Vector3(-0.704f, -0.648f, 0.533f), new Vector3(-0.981f, 0.842f, -0.712f), new Vector3(-1.074f, -0.269f, -0.467f), new Vector3(-1.074f, -0.269f, -0.467f), new Vector3(-0.747f, 0.858f, 1.102f), new Vector3(-0.747f, 0.858f, 1.102f), new Vector3(-0.981f, 0.842f, -0.712f), new Vector3(-0.747f, 0.858f, 1.102f), new Vector3(-0.57f, 1.318f, 0.094f), new Vector3(-0.57f, 1.318f, 0.094f), new Vector3(-0.981f, 0.842f, -0.712f), new Vector3(-0.419f, -0.056f, -1.633f), new Vector3(-0.479f, -1.042f, -0.483f), new Vector3(-1.074f, -0.269f, -0.467f), new Vector3(-0.419f, -0.056f, -1.633f), new Vector3(-0.353f, 1.093f, -1.109f), new Vector3(0.194f, 1.459f, -0.374f), new Vector3(0.194f, 1.459f, -0.374f), new Vector3(0.834f, 0.569f, -1.306f), new Vector3(0.666f, -1.311f, -0.187f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(-0.188f, -1.377f, 0.273f), new Vector3(-0.188f, -1.377f, 0.273f), new Vector3(0.666f, -1.311f, -0.187f), new Vector3(0.611f, 1.127f, 0.783f), new Vector3(0.802f, 0.417f, -0.35f), new Vector3(0.802f, 0.417f, -0.35f), new Vector3(0.194f, 1.459f, -0.374f), new Vector3(0.194f, 1.459f, -0.374f), new Vector3(0.611f, 1.127f, 0.783f), new Vector3(0.834f, 0.569f, -1.306f), new Vector3(1.094f, -0.488f, -1.247f), new Vector3(0.611f, 1.127f, 0.783f), new Vector3(1.009f, 0.168f, 0.567f), new Vector3(1.009f, 0.168f, 0.567f), new Vector3(0.802f, 0.417f, -0.35f), new Vector3(1.009f, 0.168f, 0.567f), new Vector3(0.835f, -0.403f, 0.352f), new Vector3(0.835f, -0.403f, 0.352f), new Vector3(0.802f, 0.417f, -0.35f), new Vector3(-0.747f, 0.858f, 1.102f), new Vector3(-0.052f, 0.617f, 1.766f), new Vector3(-0.052f, 0.617f, 1.766f), new Vector3(-0.57f, 1.318f, 0.094f), new Vector3(-0.052f, 0.617f, 1.766f), new Vector3(0.611f, 1.127f, 0.783f), new Vector3(0.611f, 1.127f, 0.783f), new Vector3(-0.57f, 1.318f, 0.094f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(-0.713f, -0.3f, 1.24f), new Vector3(-0.704f, -0.648f, 0.533f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(-0.981f, 0.842f, -0.712f), new Vector3(-0.419f, -0.056f, -1.633f), new Vector3(-0.981f, 0.842f, -0.712f), new Vector3(-0.353f, 1.093f, -1.109f), new Vector3(0.117f, 0.18f, -1.533f), new Vector3(-0.981f, 0.842f, -0.712f), new Vector3(-0.57f, 1.318f, 0.094f), new Vector3(0.194f, 1.459f, -0.374f), new Vector3(-0.353f, 1.093f, -1.109f), new Vector3(-0.57f, 1.318f, 0.094f), new Vector3(-0.713f, -0.3f, 1.24f), new Vector3(-0.747f, 0.858f, 1.102f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(-0.052f, 0.617f, 1.766f), new Vector3(-0.747f, 0.858f, 1.102f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(0.835f, -0.403f, 0.352f), new Vector3(0.666f, -1.311f, -0.187f), new Vector3(0.666f, -1.311f, -0.187f), new Vector3(1.094f, -0.488f, -1.247f), new Vector3(1.094f, -0.488f, -1.247f), new Vector3(0.835f, -0.403f, 0.352f), new Vector3(0.666f, -1.311f, -0.187f), new Vector3(-0.154f, -0.791f, -1.501f), new Vector3(1.094f, -0.488f, -1.247f), new Vector3(0.802f, 0.417f, -0.35f), new Vector3(0.802f, 0.417f, -0.35f), new Vector3(0.834f, 0.569f, -1.306f), new Vector3(-0.154f, -0.791f, -1.501f), new Vector3(-0.479f, -1.042f, -0.483f), new Vector3(-0.188f, -1.377f, 0.273f), new Vector3(-0.154f, -0.791f, -1.501f), new Vector3(0.835f, -0.403f, 0.352f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(1.009f, 0.168f, 0.567f), new Vector3(0.302f, -0.421f, 1.595f), new Vector3(1.009f, 0.168f, 0.567f), new Vector3(-0.052f, 0.617f, 1.766f)}
        );
        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine(GetType().Name, points, LineWidth, LineType.Discrete, Joins.None);
        else
            line = new VectorLine(GetType().Name, points, LineTexture, LineWidth, LineType.Discrete, Joins.None);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
        VectorManager.useDraw3D = true;

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