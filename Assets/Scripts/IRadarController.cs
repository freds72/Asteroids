using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void TargetLocked(GameObject go);

public interface IRadarController
{
	event TargetLocked OnTargetLocked;
}