using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlaneGenerator : MonoBehaviour {

    public float Size = 1.0f;
    public int GridSize = 16;

    [HideInInspector]
    public MeshFilter Filter;

    // Use this for initialization
    private void Start () {
        Filter = GetComponent<MeshFilter>();
        Filter.mesh = GenerateMesh();
    }
    private Mesh GenerateMesh() {
        Mesh m = new Mesh();
        var verticies = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        for (int x = 0; x <= GridSize; x++) {
            for (int y = 0; y <= GridSize; y++) {
                verticies.Add(new Vector3((-Size*0.5f) + (Size*(x/((float)GridSize))), 0, (-Size*0.5f) + (Size*(y/((float)GridSize))) ));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2((x/(float)GridSize), (y/(float)GridSize)));
            }
        }

        var triangles = new List<int>();
        var vertCount = GridSize + 1;

        for (int i = 0; i < ((vertCount * vertCount) - vertCount); i++) {
            if ((i+1)%vertCount == 0) { continue; }
            triangles.AddRange(new List<int>() { (i+1+vertCount), (i+vertCount), (i), (i), (i+1), (i+vertCount+1)});
        }

        m.SetVertices(verticies);
        m.SetNormals(normals);
        m.SetUVs(0, uvs);
        m.SetTriangles(triangles, 0);


        return m;
    }
}
