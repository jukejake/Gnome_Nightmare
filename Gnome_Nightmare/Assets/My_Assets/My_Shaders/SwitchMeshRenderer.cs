using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMeshRenderer : MonoBehaviour {

    public Material DepthMask;
    public Material Default;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") { this.gameObject.GetComponent<MeshRenderer>().material = DepthMask; }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") { this.gameObject.GetComponent<MeshRenderer>().material = Default; }
    }
}
