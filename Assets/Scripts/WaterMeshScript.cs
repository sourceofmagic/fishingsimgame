using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;

[RequireComponent (typeof(MeshFilter))]
public class WaterMeshScript : MonoBehaviour 
{
    #region setup

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public ushort n = 20; // разбиение нашей сеточки

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter> ().mesh = mesh;

        CreateShape();
    }
    #endregion

    void CreateShape()
    {
        vertices = createPoints().ToArray();
        triangles = createTriangles().ToArray();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    List<int> createTriangles()
    {
        List<Vector3> vert = createPoints();
        List<int> trian = new List<int>();

        for(int i = 1; i< 2*n-1; i+=2) // x
        {
            for(int j = 1; j< 2*n-1; j+=2) // z
            {
                int c = vert.IndexOf(new Vector3(i, 0, j)); // центр
                //1
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i,0,j-1))); // (1, 0)
                trian.Add(vert.IndexOf(new Vector3(i-1,0,j-1))); // (0, 0)

                //2
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i-1,0,j-1)));
                trian.Add(vert.IndexOf(new Vector3(i-1,0,j)));

                //3
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i-1,0,j)));
                trian.Add(vert.IndexOf(new Vector3(i-1,0,j+1)));

                //4
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i-1,0,j+1)));
                trian.Add(vert.IndexOf(new Vector3(i,0,j+1)));

                //5
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i,0,j+1)));
                trian.Add(vert.IndexOf(new Vector3(i+1,0,j+1)));

                //6
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i+1,0,j+1)));
                trian.Add(vert.IndexOf(new Vector3(i+1,0,j)));

                //7
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i+1,0,j)));
                trian.Add(vert.IndexOf(new Vector3(i+1,0,j-1)));

                //8
                trian.Add(c);
                trian.Add(vert.IndexOf(new Vector3(i+1,0,j-1)));
                trian.Add(vert.IndexOf(new Vector3(i,0,j-1)));
            }
        }
        return trian;
    }

    List<Vector3> createPoints() // распределение точек в соответствие с n
    {
        List<Vector3> v = new List<Vector3>();
        for(int i = 0; i < 2*n; ++i)
        {
            for(int j = 0; j<2*n; ++j)
            {
                v.Add(new Vector3(i, 0, j));
            }
        }
        return v;
    }
}
