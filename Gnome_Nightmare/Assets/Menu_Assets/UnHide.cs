using UnityEngine;

public class UnHide : MonoBehaviour {

    public void HideUnHide() {
        if (this.gameObject.activeSelf) { this.gameObject.SetActive(false); }
        else { this.gameObject.SetActive(true); }
    }
}
