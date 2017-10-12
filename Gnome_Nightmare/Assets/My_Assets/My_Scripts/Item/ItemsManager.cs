using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour {


    public static ItemsManager instance;
    void Awake() { instance = this; }
    public GameObject Items;


    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart(){

    }
}
