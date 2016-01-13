using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class TileMap : MonoBehaviour {

    public int HTileStart = 200;
    public int HTileEnd = 250;
    public int VTileStart = 300;
    public int VTileEnd = 305;
    public GameObject TilePrefab;

	// Use this for initialization
	void Start () {
	    for(int i=HTileStart;i<=HTileEnd;i++)
        {
            for(int j=VTileStart;j<=VTileEnd;j++)
            {
                Sprite sprite = Resources.Load<Sprite>(string.Format("Map/{0}.{1}", i, j));
                float tilesize = sprite.texture.width / sprite.pixelsPerUnit;
                GameObject go = (GameObject)Instantiate(TilePrefab, new Vector3(i * tilesize, -j * tilesize,0), Quaternion.identity);
                go.transform.SetParent(transform, false);
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
