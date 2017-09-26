﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If_Under_Floor : MonoBehaviour {

    GameObject IfUnderFloor;
    // Use this for initialization
    void Start () {
        IfUnderFloor = GameObject.Find("Floor").gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        if (IfUnderFloor.transform.position.y - 25.0f > this.transform.position.y) {
            this.transform.SetPositionAndRotation(new Vector3(this.transform.position.x,(IfUnderFloor.transform.position.y + IfUnderFloor.transform.localScale.y + 10.0f), this.transform.position.z), this.transform.rotation);
            this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
	}
}
