using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Slowly : MonoBehaviour {

    public float RotationSpeed = 1.0f;
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = new Vector3(0.0f, RotationSpeed, 0.0f);
        this.transform.Rotate(rot);
	}
}
