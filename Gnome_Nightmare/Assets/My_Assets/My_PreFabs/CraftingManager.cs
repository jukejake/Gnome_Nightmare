using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {

    public static CraftingManager instance;
    void Awake() { instance = this; }
    [Space]
    //Output
    public GameObject Output_Slot;
    //Modifier
    public GameObject Modifier_Slot;
    [Space]
    //Combined
    public GameObject Combined_First_Slot;
    public GameObject Combined_Second_Slot;
    [Space]
    //Disassemble
    public GameObject Disassemble_First_Slot;
    public GameObject Disassemble_Second_Slot;
    [Space]
    private bool hello;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
