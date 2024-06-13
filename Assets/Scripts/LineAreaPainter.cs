using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineAreaPainter : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;

        // Create Mesh Components
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mesh = new Mesh();
        

        meshRenderer.material = new Material(Shader.Find("Sprites/Default")); // Or assign a custom material
        meshRenderer.material.color = Color.red; // Change color as needed
    }

    void Update()
    {
        GenerateMesh();
        meshFilter.mesh = mesh;
    }

    void GenerateMesh()
    {
        int positionCount = lineRenderer.positionCount;
        Vector3[] positions = new Vector3[positionCount];
        lineRenderer.GetPositions(positions);

        Vector3[] vertices = new Vector3[positionCount * 2];
        int[] triangles = new int[(positionCount - 1) * 6];

        // Create vertices for the top and bottom of the lines
        for (int i = 0; i < positionCount; i++)
        {
            Vector3 offset = Vector3.right * 0.1f; // Adjust based on the width you want

            vertices[i * 2] = positions[i] - offset;
            vertices[i * 2 + 1] = positions[i] + offset;
        }

        // Create triangles
        for (int i = 0; i < positionCount - 1; i++)
        {
            int vertIndex = i * 2;
            int triIndex = i * 6;

            triangles[triIndex] = vertIndex;
            triangles[triIndex + 1] = vertIndex + 1;
            triangles[triIndex + 2] = vertIndex + 2;

            triangles[triIndex + 3] = vertIndex + 1;
            triangles[triIndex + 4] = vertIndex + 3;
            triangles[triIndex + 5] = vertIndex + 2;
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

