using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SliceScroller : MonoBehaviour {

    public Camera ScrollCamera;
    public int BlockSize = 1;

    public WallSlice SlicePrefab;
    public Vector3 WallOffset = new Vector3(0, 0, -2);
    public Sprite[] WallTextures;
    public Sprite[] FloorTextures;

    List<WallSlice> _slices = new List<WallSlice>();
    Vector3 position;
    float _height;
    float _width;

    /// <summary>
    /// Assigns a random floor + wall texture to a slice
    /// </summary>
    /// <param name="slice"></param>
    void RandomizeTextures(WallSlice slice)
    {
        slice.Wall.sprite = WallTextures[Random.Range(0, WallTextures.Length)];
        slice.Floor.sprite = FloorTextures[Random.Range(0, FloorTextures.Length)];
    }

    // Use this for initialization
    void Start()
    {
        if ( ScrollCamera == null )
            ScrollCamera = Camera.main;

        position = transform.position;

        _height = 2f * Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        // create the wall
        float x = -(_width / 2) - BlockSize;
        int n = Mathf.FloorToInt(_width / BlockSize) + 1;
        Debug.Log(string.Format("Width: {0} -> Slices: {1}", _width, n));
        for (int i = 0; i < n; i++)
        {
            WallSlice slice = (WallSlice)Instantiate(SlicePrefab, new Vector3(WallOffset.x + x, WallOffset.y, WallOffset.z), Quaternion.identity);
            _slices.Add(slice);
            // pick a random wall texture
            RandomizeTextures(slice);
            x += BlockSize;
        }
    }

    // move slices back and forth
    void Update()
    {
        WallSlice firstChild = _slices.FirstOrDefault();

        if (firstChild != null)
        {
            if (firstChild.transform.position.x < ScrollCamera.transform.position.x - _width / 2 - BlockSize)
            {
                WallSlice lastChild = _slices[_slices.Count - 1];
                Vector3 lastPosition = lastChild.transform.position;

                // take last child position + blocksize offset
                firstChild.transform.position = new Vector3(lastPosition.x + BlockSize, lastPosition.y, lastPosition.z);

                _slices.RemoveAt(0);
                // put back wall slice + randomize texture
                RandomizeTextures(firstChild);
                // push back
                _slices.Add(firstChild);
            }
        }
    }
}
