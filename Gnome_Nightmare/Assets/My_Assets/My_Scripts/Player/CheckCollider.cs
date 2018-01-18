using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour {

    public bool IsTriggered = false;

    private void OnTriggerStay(Collider other) {
        IsTriggered = true;
    }
    private void OnTriggerExit(Collider other) {
        IsTriggered = false;
    }
}
