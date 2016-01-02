using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(CircleCollider2D))]
[ExecuteInEditMode]
public class Asteroid : MonoBehaviour {

    public float MinRadius = 1;
    public float MaxRadius = 1.5f;
    public int Precision = 8;

    Mesh MakeCircle(out float avgRadius)
    {
        float angleStep = 2 * Mathf.PI / (float)Precision;
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        // Make first triangle.
        vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
        float angle = 0;
        avgRadius = 0;
        for (int i = 0; i < Precision;i++ )
        {
            angle += angleStep;
            float radius =  Random.Range(MinRadius, MaxRadius);
            vertexList.Add(new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0.0f));     // 3. First vertex on circle outline rotated by angle)          
            avgRadius += radius;
        }
        // Add triangle indices.        
        for (int i = 1; i <= Precision; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(i);
            triangleList.Add(i%Precision + 1);
        }
        var mesh = new Mesh();
        mesh.vertices = vertexList.ToArray();
        avgRadius /= Precision;
        mesh.triangles = triangleList.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }
    
    void Awake()
    {
        float avgRadius;
        Mesh mesh = MakeCircle(out avgRadius);
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<CircleCollider2D>().radius = avgRadius;        
    }

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
