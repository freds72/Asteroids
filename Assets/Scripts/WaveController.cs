using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class WaveAction
{
    // The different Enemy levels
    public enum EnemyLevels
    {
        Easy,
        Medium,
        Hard,
        Hazard,
        Boss,
        Furniture
    }

    public float Delay;
    public int SpawnCount = 1;
    [Tooltip("Entity type (easy, medium, ...), boss require an explicit prefab")]
    public EnemyLevels SpawnLevel;
    [Tooltip("Boss prefab")]
    public GameObject BossPrefab;
    [Tooltip("Lane index. -1 for a random location")]
    public int Lane = -1;
    public enum SpawnLocation
    {
        Right,
        Left
    }
    public SpawnLocation Location = SpawnLocation.Right;
}

[System.Serializable]
public class Wave
{
    public string Name;
    [Tooltip("Delay after the last enenmy of previous wave is dead.")]
    public float Delay;
    public WaveAction[] Actions;
}

/// <summary>
/// Handle all logic related to spawning NPC's
/// </summary>
public class WaveController : MonoBehaviour {

    [Tooltip("Easy enemies")]
    public GameObject[] EasyPrefabs;
    [Tooltip("Medium enemies")]
    public GameObject[] MediumPrefabs;
    [Tooltip("Hard enemies")]
    public GameObject[] HardPrefabs;
    [Tooltip("Static things")]
    public GameObject[] FurniturePrefabs;
    [Tooltip("Static things that blow up")]
    public GameObject[] HazardPrefabs;

    public Wave[] Waves;
    public Transform LeftSpawnPoint;
    public Transform RightSpawnPoint;

    public int LaneCount = 8;
    public int BlockSize = 1;
    List<HitPoints> _activeNPCs = new List<HitPoints>();

    public Wave CurrentWave { get; private set; }

    private class WaveState
    {
        public bool IsRunning { get; set; }
    }

    private class Lanes
    {
        bool[] _left;
        bool[] _right;

        public struct LaneLock : IDisposable
        {
            bool[] _lane;
            int _index;

            public int Index { get {return _index; } }

            public LaneLock(bool[] lane, int index)
            {
                _lane = lane;
                _index = index;
                _lane[_index] = true;
            }
            
            public void Dispose()
            {
                _lane[_index] = false;
            }
        }

        public Lanes(int count)
        {
            _left = new bool[count];
            _right = new bool[count];
        }

        public void Reset()
        {
            Array.Clear(_left, 0, _left.Length);
            Array.Clear(_right, 0, _right.Length);
        }

        public LaneLock AcquireLane(int laneHint, WaveAction.SpawnLocation location)
        {
            bool[] lane = null;
            switch(location)
            {
                case WaveAction.SpawnLocation.Left:
                    lane = _left;
                    break;
                case WaveAction.SpawnLocation.Right:
                    lane = _right;
                    break;
            }
            if (laneHint != -1)
                return new LaneLock(lane, laneHint);

            // count number of free slots
            int freeSlots = lane.Count(l => l == false);
            int slot = UnityEngine.Random.Range(0, freeSlots);
            for (int i = 0; i < lane.Length; i++)
            {
                // skip free slots
                if (lane[i] == false)
                    slot--;
                // found the right slot!
                if (slot == 0)
                    return new LaneLock(lane, i);
            }
            throw new InvalidOperationException("No lane left.");
        }        
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(CreateWaves());
	}


    GameObject GetPrefab(WaveAction action)
    {
        GameObject[] prefabs = null;
        switch(action.SpawnLevel)
        {
            case WaveAction.EnemyLevels.Easy:
                prefabs = EasyPrefabs;
                break;
            case WaveAction.EnemyLevels.Medium:
                prefabs = MediumPrefabs;
                break;
            case WaveAction.EnemyLevels.Hard:
                prefabs = HardPrefabs;
                break;
            case WaveAction.EnemyLevels.Furniture:
                prefabs = FurniturePrefabs;
                break;
            case WaveAction.EnemyLevels.Hazard:
                prefabs = HazardPrefabs;
                break;
        }
        return prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
    }

    IEnumerator Spawn(WaveAction action, Lanes lanes, WaveState state)
    {
        state.IsRunning = true;
        for (int i = 0;i<action.SpawnCount; i++)
        {
            using(Lanes.LaneLock lck = lanes.AcquireLane(action.Lane,action.Location))
            { 
                float z = lck.Index * BlockSize;
                Vector3 spawnLocation = Vector3.zero;
                switch (action.Location)
                {
                    case WaveAction.SpawnLocation.Right:
                        spawnLocation = RightSpawnPoint.position;
                        break;
                    case WaveAction.SpawnLocation.Left:
                        spawnLocation = LeftSpawnPoint.position;
                        break;
                }
                GameObject go = (GameObject)Instantiate(
                    action.BossPrefab == null ? GetPrefab(action) : action.BossPrefab,
                    new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z - z),
                    Quaternion.identity);
                HitPoints hp = go.GetComponent<HitPoints>();
                // wait for the enemy to die before continuing
                if ( hp != null )
                    while (!hp.IsDead)
                        yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(action.Delay);
        }
        state.IsRunning = false;
    }

    IEnumerator CreateWaves()
    {
        Lanes lanes = new Lanes(LaneCount);
        // iterates over all waves
        foreach(Wave it in Waves)
        {
            yield return new WaitForSeconds(it.Delay);

            CurrentWave = it;
            lanes.Reset();
            List<WaveState> waitEvents = new List<WaveState>();
            foreach (WaveAction action in it.Actions)
            {
                WaveState ws = new WaveState();
                StartCoroutine(Spawn(action, lanes, ws));
            }
            // wait until all events are over
            yield return new WaitUntil(() => waitEvents.Count(we => we.IsRunning == true) == 0);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
