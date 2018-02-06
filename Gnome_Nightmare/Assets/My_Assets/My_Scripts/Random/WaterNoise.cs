using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNoise : MonoBehaviour {
    public float Power = 3;
    private float Scale = 0.3f; //Don't change (pairs with TileOffset)
    public float TimeScale = 1;

    private float TileOffset = 15; //Don't change (pairs with Scale)

    private float OffsetX;
    private float OffsetY;
    private MeshFilter mf;

    private void Start() {
        mf = GetComponent<MeshFilter>();
        MakeNoise();
        OffsetX = Time.deltaTime * (this.gameObject.transform.position.x* TileOffset);
        OffsetY = Time.deltaTime * (this.gameObject.transform.position.z* TileOffset);
    }
    private void Update() {
        MakeNoise();
        OffsetX += Time.deltaTime * TimeScale;
        OffsetY += Time.deltaTime * TimeScale;
    }

    private void MakeNoise() {
        Vector3[] verticies = mf.mesh.vertices;
        for (int i = 0; i < verticies.Length; i++) {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * Power;
        }
        mf.mesh.vertices = verticies;
    }
    private float CalculateHeight(float x, float y) {
        float xCord = x * Scale + OffsetX;
        float yCord = y * Scale + OffsetY;

        return Mathf.PerlinNoise(xCord,yCord);
    }
}
