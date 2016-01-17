using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Dial : MonoBehaviour {
    public float LineWidth = 1;
    public int Precision = 16;
    public int Padding = 2;
    public float AlertRatio = 0.9f;
    public Texture NormalTexture = null;
    public Texture AlertTexture = null;
    VectorLine _dialNormal;
    VectorLine _dialAlert;
    VectorLine _arrowLine;
    float _width;

	// Use this for initialization
	void Start () {
        var rectTransform = GetComponent<RectTransform>();
        _width = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) - Padding;

        _dialNormal = new VectorLine("NormalDial", new List<Vector2>(Precision), NormalTexture, LineWidth, LineType.Continuous, Joins.Weld);
        _dialNormal.MakeArc(rectTransform.rect.center, _width / 2, _width / 2, 90, 360 - (1 - AlertRatio) * 360);
        _dialNormal.drawTransform = transform; 
        _dialNormal.Draw();

        _dialAlert = new VectorLine("AlertDial", new List<Vector2>(Precision), AlertTexture, LineWidth * 1.1f, LineType.Continuous, Joins.Weld);
        _dialAlert.MakeArc(rectTransform.rect.center, _width / 2, _width / 2, 360 - (1 - AlertRatio) * 360, 360);
        _dialAlert.drawTransform = transform;
        _dialAlert.Draw();

        var points = new List<Vector2>(new Vector2[] { rectTransform.rect.center, new Vector2(_width / 2, 0) }); 
        _arrowLine = new VectorLine("ArrowLine", points, NormalTexture, LineWidth); 
        _arrowLine.drawTransform = transform;
        _arrowLine.Draw(); 
    }
	
	// Update is called once per frame
	void Update () {
        float angle = Time.time * 180 / Mathf.PI / 8;
        _arrowLine.points2[1] = new Vector2(_width * Mathf.Cos(angle) / 2, _width * Mathf.Sin(angle) / 2);
        _arrowLine.Draw();
        _dialNormal.Draw();
        _dialAlert.Draw();
	}
}
