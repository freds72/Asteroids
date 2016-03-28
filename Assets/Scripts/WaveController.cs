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

    [Tooltip("Delay to spawn another entity")]
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
    [Tooltip("Distance required to spawn wave")]
    public float Distance;

    public WaveAction[] Actions;
}

/// <summary>
/// Handle all logic related to spawning NPC's
/// </summary>
public class WaveController : MonoBehaviour {

    public ConstantCameraTranslation CameraController;

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
        int _count = 0;
        public void Lock()
        {
            _count++;
        }

        public void Unlock()
        {
            _count--;
        }
        public bool IsLocked { get { return _count > 0; } }
        public void Reset() { _count = 0; }
    }


	// Use this for initialization
	void Start () {
        CameraController.enabled = false;
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

    IEnumerator Spawn(WaveAction action, WaveState state)
    {
        state.Lock();
        yield return new WaitForSeconds(action.Delay);

        int lane = action.Lane;
        if (lane == -1)
            lane = UnityEngine.Random.Range(0, LaneCount);

        float z = lane * BlockSize;
        Vector3 spawnLocation = new Vector3(UnityEngine.Random.Range(0.5f,0.5f),0,UnityEngine.Random.Range(-0.5f,0.5f));
        switch (action.Location)
        {
            case WaveAction.SpawnLocation.Right:
                spawnLocation += RightSpawnPoint.position;
                break;
            case WaveAction.SpawnLocation.Left:
                spawnLocation += LeftSpawnPoint.position;
                break;
        }
        GameObject go = (GameObject)Instantiate(
            action.BossPrefab == null ? GetPrefab(action) : action.BossPrefab,
            new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z + z),
            Quaternion.identity);
        HitPoints hp = go.GetComponent<HitPoints>();
        // wait for the enemy to die before continuing
        if (hp != null)
            yield return new WaitUntil(() => hp.IsDead);

        state.Unlock();
    }

    IEnumerator CreateWaves()
    {
        WaveState ws = new WaveState();
        // iterates over all waves
        foreach(Wave it in Waves)
        {
            // allow scrolling
            CameraController.enabled = true;

            // delay between 2 waves
            // TODO: transform into meters!!!!
            yield return new WaitUntil(() => CameraController.Distance > it.Distance);
            
            // disable scrolling while fighting
            CameraController.enabled = false;

            CurrentWave = it;
            ws.Reset();
            foreach (WaveAction action in it.Actions)
            {
                for (int i = 0; i < action.SpawnCount; i++)
                {                    
                    StartCoroutine(Spawn(action, ws));
                }
            }
            // wait until all events are over
            yield return new WaitWhile(() => ws.IsLocked);
        }
    }
}
