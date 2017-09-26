using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {

    public Output LoadText;
    public string NameOfNPC;
    public string FileName;
    public string DialogueOfNPC;
    public GameObject Dialogue_Prefab;
    
    private bool MenuOpen = false;

    private void Start() {
        FileName = "Hello";
        LoadText = new Output();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "NPC" && Input.GetButton("E") && MenuOpen == false) {
            MenuOpen = true;
            if (other.transform.Find("Dialogue_Menu") == true) {
                Debug.Log("It already there");
                DistoryNpcDialogue();
            }
            else {
                LoadNpcDialogue(other);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        DistoryNpcDialogue();
    }

    private void LoadNpcDialogue(Collider other) {
        NameOfNPC = other.name;
        DialogueOfNPC = LoadText.ReadText(NameOfNPC, FileName);
        //Debug.Log(DialogueOfNPC);

        GameObject Dialogue_Menu = (GameObject)Instantiate(Dialogue_Prefab);
        Dialogue_Menu.transform.SetParent(other.transform);
        Dialogue_Menu.name = "Dialogue_Menu";

        Dialogue_Menu.transform.Find("Dialogue_Box").transform.Find("NPCName").GetComponent<Text>().text = NameOfNPC;
        Dialogue_Menu.transform.Find("Dialogue_Box").transform.Find("NPCDialogue").GetComponent<Text>().text = DialogueOfNPC;
    }

    private void DistoryNpcDialogue() {
        Destroy(GameObject.Find("Dialogue_Menu").gameObject);
        MenuOpen = false;
    }

    public void ContinueDialogue() {
        FileName = "Q_001";
        //Output hello = new Output();
        //Debug.Log(hello.ReadText(NameOfNPC, FileName));
        DialogueOfNPC = LoadText.ReadText(NameOfNPC, FileName);
        GameObject.Find("Dialogue_Menu").gameObject.transform.Find("Dialogue_Box").transform.Find("NPCDialogue").GetComponent<Text>().text = DialogueOfNPC;
    }
}
