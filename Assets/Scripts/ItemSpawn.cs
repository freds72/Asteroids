using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class ItemSpawn : MonoBehaviour
{
    public InventoryItem Item { get; set; }
    MessageTicker _ticker;
    float _startTime;
    int _id;

    void Start()
    {
        _ticker = GameObject.FindGameObjectWithTag("TickerSingleton").GetComponent<MessageTicker>();
        _startTime = Time.time;
        Invoke("Spawn", Item.SpawnTime);
        _id = _ticker.Create();
    }

    void Update()
    {
        _ticker.Update(_id, string.Format("{0} : {1:0.00}s", Item.Label, Mathf.Max(Item.SpawnTime - (Time.time - _startTime),0)));
    }

    void Spawn()
    {
        if ( Item.Prefab != null)
            Instantiate(Item.Prefab, transform.position, Quaternion.identity);

        _ticker.Release(_id);
        
        Destroy(gameObject);
    }
}
