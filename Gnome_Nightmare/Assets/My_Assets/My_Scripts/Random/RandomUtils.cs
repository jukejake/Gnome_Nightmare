using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUtils : MonoBehaviour
{
    static public Vector3 RandomVector3InBox(Vector3 maxPosition) {
        Vector3 minPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
        return randomPosition;
    }
    static public Vector3 RandomVector3InBox(Vector3 minPosition, Vector3 maxPosition) {
        Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
        return randomPosition;
    }
}
