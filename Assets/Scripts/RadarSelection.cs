using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using UnityEngine.UI;

public class RadarSelection : MonoBehaviour {
    public float LineWidth = 1;
    public Texture LineTexture = null;
    public float Size = 1;

    public Text InformationPanel;
    public float InformationDelay = 0.5f;

    public string TargetInfo
    {
        get;
        set;
    }

	// Use this for initialization
	void Start () {
        float hsize = Size / 2;
        float msize = Size / 3;
        List<Vector3> points = new List<Vector3>(new Vector3[] { 
            // left
            new Vector3(-msize,hsize,0),
            new Vector3(-hsize,hsize,0),
            new Vector3(-hsize,hsize,0),
            new Vector3(-hsize,-hsize,0),
            new Vector3(-hsize,-hsize,0),
            new Vector3(-msize,-hsize,0),
            // right
            new Vector3(msize,hsize,0),
            new Vector3(hsize,hsize,0),
            new Vector3(hsize,hsize,0),
            new Vector3(hsize,-hsize,0),
            new Vector3(hsize,-hsize,0),
            new Vector3(msize,-hsize,0)});
        VectorLine line;
        if (LineTexture == null)
            line = new VectorLine("RadarSelection", points, LineWidth, LineType.Discrete, Joins.Weld);
        else
            line = new VectorLine("RadarSelection", points, LineTexture, LineWidth, LineType.Discrete, Joins.Weld);

        VectorManager.ObjectSetup(gameObject, line, Visibility.Dynamic, Brightness.None);
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnEnable()
    {
        StartCoroutine(UpdateInformationPanel());
    }

    IEnumerator UpdateInformationPanel()
    {
        if (InformationPanel != null)
        {
            while (true)
            {
                InformationPanel.text = TargetInfo;
                yield return new WaitForSeconds(InformationDelay);
            }
        }
    }
}
