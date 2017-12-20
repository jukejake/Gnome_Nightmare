using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour {

    public Transform bone;
    public Transform cameraTarget;

    public Transform HitBox;
    void LateUpdate() {
        bone.LookAt(cameraTarget);
        HitBox.LookAt(cameraTarget);
    }
}
