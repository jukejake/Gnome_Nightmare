using UnityEngine;

public class SwitchActive : MonoBehaviour {
    //Will switch active state
    public void Switch() {
        if (this.gameObject.activeSelf) { this.gameObject.SetActive(false); }
        else { this.gameObject.SetActive(true); }
    }
}
