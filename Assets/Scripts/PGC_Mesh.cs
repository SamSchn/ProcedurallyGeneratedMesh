using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Mesh : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Rigidbody rb;
    private Vector3[] vertices;
    public int xSize;
    public int ySize;
    

    private void Awake()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("MeshMaterial");

        //add a collider
        meshCollider = gameObject.AddComponent<MeshCollider>();
        rb = gameObject.AddComponent<Rigidbody>();
        Init_MeshBuilder();

    }

    private void Update()
    {
        MeshWaves();
    }

    void Init_MeshBuilder()
    {
        
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody cubeRb = cube.AddComponent<Rigidbody>();
        cubeRb.position = new Vector3(xSize / 2, 10, ySize / 2);

        //setup mesh
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        mesh.name = "Procedural Grid";

        //add the mesh collider
        meshCollider = GetComponent<MeshCollider>();

        //add the rigid body
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, 1, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }

        //add the vertices to the mesh
        mesh.vertices = vertices;
        mesh.uv = uv;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, vi++, ti += 6)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        //add the triangles to the mesh
        mesh.triangles = triangles;

        //need to recalculate normal for better lighting
        mesh.RecalculateNormals();

        //attach collider
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = false;

    }

    void MeshWaves()
    {
        //setup mesh
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        mesh.name = "Procedural Grid";

        //add the mesh collider
        meshCollider = GetComponent<MeshCollider>();

        //add the rigid body
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, (Mathf.Sin(x + Time.time) + Mathf.Cos(y + 0.5f * Time.time + x)) * Mathf.Sin(0.2f * Time.time) + Mathf.Cos(y + Time.time), y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }
    

        //add the vertices to the mesh
        mesh.vertices = vertices;
        mesh.uv = uv;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, vi++, ti += 6)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        //add the triangles to the mesh
        mesh.triangles = triangles;

        //need to recalculate normal for better lighting
        mesh.RecalculateNormals();

        //attach collider
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}

