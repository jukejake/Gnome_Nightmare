using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager instance;
    void Awake() { instance = this; }
    public GameObject Menu;


    // Use this for initialization
    void Start() {
        EnableGraphicRaycaster(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void EnableGraphicRaycaster(bool enable) {
        Menu.GetComponent<GraphicRaycaster>().enabled = enable;
        //if (Menu.GetComponent<GraphicRaycaster>().enabled) { Menu.GetComponent<GraphicRaycaster>().enabled = false; }
        //else { Menu.GetComponent<GraphicRaycaster>().enabled = true; }
    }
}
