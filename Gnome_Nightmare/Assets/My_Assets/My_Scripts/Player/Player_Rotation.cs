using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour {

    public Transform bone;
    public Transform cameraTarget;
    public Transform HitBox;


    private void Start() {
        if (cameraTarget == null) { cameraTarget = GameObject.Find("Main Camera").transform.GetChild(0).transform; }
    }

    void LateUpdate() {
        bone.LookAt(cameraTarget);
        HitBox.LookAt(cameraTarget);
    }
}
