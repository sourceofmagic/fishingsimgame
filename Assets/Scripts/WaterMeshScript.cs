using UnityEngine;
using System.Collections.Generic;


[RequireComponent (typeof(MeshFilter))]
public class WaterMeshScript : MonoBehaviour 
{
    #region setup

    Mesh mesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    public ushort n = 55; // разбиение нашей сеточки

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "MyWaterMesh";
        GetComponent<MeshFilter> ().mesh = mesh;

        CreateShape();

        meshFilter = GetComponent<MeshFilter> ();
        meshFilter.mesh = mesh;
        meshCollider = GetComponent<MeshCollider> ();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
    #endregion

    void CreateShape()
    {
        /*vertices = createPoints().ToArray();
        triangles = createTriangles().ToArray();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;*/

        // новое ==================================

        int width = 2 * n;
        int totalVerts = width * width;
        vertices = new Vector3[totalVerts];
        uvs = new Vector2[totalVerts];

        // заполняем вершины и uv

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < width; z++)
            {
                int idx = x * width + z;
                vertices[idx] = new Vector3(x, 0f, z);
                uvs[idx] = new Vector2((float)x / (width - 1), (float)z / (width - 1));
            }
        }

        // кол-во центров и треугов
        int centersPerRow = (width - 1) / 2; // кол-во центров по одному направлению (i=1,3,...,width-2)
        int centersTotal = centersPerRow * centersPerRow;
        int trisPerCenter = 8;               // по 8 треугольников вокруг центра
        int indicesCount = centersTotal * trisPerCenter * 3;

        List<int> triList = new List<int>(indicesCount);

        // 3) Пробегаем центры: i и j — координаты центра (не индекс массива)
        // центры на нечетных координатах: 1,3,5,...,width-2
        for (int i = 1; i < width - 1; i += 2)    // i = x
        {
            for (int j = 1; j < width - 1; j += 2) // j = z
            {
                // индекс центра в массиве вершин
                int c = i * width + j;

                // соседние индексы (вокруг центра)
                int a = i * width + (j - 1);       // (i, j-1)
                int b = (i - 1) * width + (j - 1); // (i-1, j-1)
                int d = (i - 1) * width + j;       // (i-1, j)
                int e = (i - 1) * width + (j + 1); // (i-1, j+1)
                int f = i * width + (j + 1);       // (i, j+1)
                int g = (i + 1) * width + (j + 1); // (i+1, j+1)
                int h = (i + 1) * width + j;       // (i+1, j)
                int k = (i + 1) * width + (j - 1); // (i+1, j-1)

                // добавляем 8 треугольников вокруг центра
                triList.Add(c); triList.Add(a); triList.Add(b);
                triList.Add(c); triList.Add(b); triList.Add(d);
                triList.Add(c); triList.Add(d); triList.Add(e);
                triList.Add(c); triList.Add(e); triList.Add(f);
                triList.Add(c); triList.Add(f); triList.Add(g);
                triList.Add(c); triList.Add(g); triList.Add(h);
                triList.Add(c); triList.Add(h); triList.Add(k);
                triList.Add(c); triList.Add(k); triList.Add(a);
            }
        }

        triangles = triList.ToArray();

        // 4) Присваиваем мешу
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        // 5) Нормали/границы/тангенсы
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        // кон =======================================
    }

/*    List<int> createTriangles()
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
    }*/
}
