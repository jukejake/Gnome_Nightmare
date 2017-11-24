using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour {

    public Transform bone;
    public Transform cameraTarget;

    void LateUpdate()
    {
        bone.LookAt(cameraTarget);
    }
}
