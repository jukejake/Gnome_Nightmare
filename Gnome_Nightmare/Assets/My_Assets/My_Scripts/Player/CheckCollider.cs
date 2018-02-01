using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour {

    public bool IsTriggered = false;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 2) { IsTriggered = false; }
        else { IsTriggered = true; }
    }
    private void OnTriggerExit(Collider other) {
        IsTriggered = false;
    }
}
