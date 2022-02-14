using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygons : MonoBehaviour
{
    [Range(3, 1000)]
    public int noOfSides;
    [Range(1, 10)]
    public float sidelength;
    private float oneUnEqualAngle, otherEqualangle;
    private float halfLengthOfTriangle;
    private float radiusOfPolygon;
    private Vector3[] polygonVertices = Array.Empty<Vector3>();
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    int[] polyTriangles = Array.Empty<int>();

    private void Start()
    {


        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        meshFilter = gameObject.GetComponent<MeshFilter>();
        CreatePolygonMesh();
    }


    public void CreatePolygonMesh()
    {


        mesh = new Mesh();
        polygonVertices = new Vector3[noOfSides];
        polyTriangles = new int[(noOfSides - 2) * 3];
        oneUnEqualAngle = (float)360 / noOfSides;

        float oneUnEqualAngleRadians = oneUnEqualAngle * Mathf.Deg2Rad;
        otherEqualangle = (float)(180 - oneUnEqualAngle) / 2;
        halfLengthOfTriangle = (float)sidelength / 2;
        float otherEqualangleRadians = otherEqualangle * Mathf.Deg2Rad;
        radiusOfPolygon = halfLengthOfTriangle / Mathf.Cos(otherEqualangleRadians);




        polygonVertices[0] = new Vector3(0, radiusOfPolygon, 0);
        float previosXcoord = polygonVertices[0].x;
        float previosYcoord = polygonVertices[0].y;

        for (int i = 1; i < noOfSides; i++)
        {

            float nextXcoord = previosXcoord * Mathf.Cos(oneUnEqualAngleRadians) + previosYcoord * Mathf.Sin(oneUnEqualAngleRadians);
            float nextYcoord = -(previosXcoord * Mathf.Sin(oneUnEqualAngleRadians)) + previosYcoord * Mathf.Cos(oneUnEqualAngleRadians);

            polygonVertices[i] = new Vector3(nextXcoord, nextYcoord, 0);
            previosXcoord = nextXcoord;
            previosYcoord = nextYcoord;



        }

        polyTriangles[0] = 0;
        polyTriangles[1] = 1;
        polyTriangles[2] = 2;
        int firstIndex = polyTriangles[0];
        int secondIndex = polyTriangles[1];
        int thirdIndex = polyTriangles[2];

        for (int j = 3; j < polyTriangles.Length; j = j + 3)
        {
            polyTriangles[j] = firstIndex;
            polyTriangles[j + 1] = secondIndex + 1;
            polyTriangles[j + 2] = thirdIndex + 1;
            secondIndex = polyTriangles[j + 1];
            thirdIndex = polyTriangles[j + 2];
        }


        mesh.vertices = polygonVertices;
        mesh.triangles = polyTriangles;
        meshFilter.mesh = mesh;


    }
}
