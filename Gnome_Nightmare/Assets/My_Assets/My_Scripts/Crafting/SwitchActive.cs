using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActive : MonoBehaviour {

    public void Switch() {
        if (this.gameObject.activeSelf) {
            this.gameObject.SetActive(false);
        }
        else { this.gameObject.SetActive(true); }
    }
}
