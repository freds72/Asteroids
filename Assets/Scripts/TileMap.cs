using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class TileMap : MonoBehaviour {

    public int Level = 11;
    public int LatStart = 758;
    public int LatEnd = 811;
    public int LongStart = 313;
    public int LongEnd = 380 ;
    public GameObject TilePrefab;
    const int TILE_SIZE = 256;

	// Use this for initialization
	void Start () {
        float scale = 156543.03392f / Mathf.Pow(2, Level);
        float tilesize = TILE_SIZE / (1000 / scale); // in world units
        float latOffset = (LatStart + (LatEnd - LatStart)/2) * tilesize;
        float longOffset = (LongStart + (LongEnd - LongStart)/2) * tilesize;
	    for(int lat=LatStart;lat<=LatEnd;lat++)
        {
            for(int lng=LongStart;lng<=LongEnd;lng++)
            {
                Sprite sprite = Resources.Load<Sprite>(string.Format("Map/{2}/{0}.{1}", lat, lng, Level));
                if (sprite != null)
                {
                    GameObject go = (GameObject)Instantiate(TilePrefab, new Vector3(lng * tilesize - longOffset, latOffset - lat * tilesize, 0), Quaternion.identity);
                    go.transform.SetParent(transform, false);
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.sprite = sprite;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
