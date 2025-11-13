using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    #region setup
    public Mesh mesh;
    public Vector3[] polygonPoints; //все точки меша
    public int[] polygonTriangles; // все треугольники меша


    public int polygonSides = 4;
    public float polygonRadius = 7.5f;
    public byte centerRadius;

    private void Start()
    {
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        DrawFilled(polygonSides, polygonRadius);
    }
    #endregion

    void DrawFilled(int sides, float radius)
    {
        polygonPoints = GetCircumferencePoints(sides,radius).ToArray();
        polygonTriangles = DrawFilledTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
    }

    List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; ++i)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, 0));
        }
        return points;

    }

    int[] DrawFilledTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length - 2;
        List <int> newTriangles = new List <int>();
        for(int i =0; i<triangleAmount; ++i)
        {
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }
        return newTriangles.ToArray();
    }
}
