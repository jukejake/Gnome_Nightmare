using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public Dialogue NPCDialogue;
    public Output LoadText;
    public int TextNumber;
    public GameObject Dialogue_Prefab;
    
    private bool MenuOpen = false;

    private void Start() {
        LoadText = new Output();
        TextNumber = 0;
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "NPC" && Input.GetButton("E") && MenuOpen == false) {
            MenuOpen = true;
            if (other.transform.Find("Dialogue_Menu") == true) {
                Debug.Log("It's already there.");
                DistoryNpcDialogue();
            }
            else {
                GameObject Dialogue_Menu = (GameObject)Instantiate(Dialogue_Prefab);
                Dialogue_Menu.transform.SetParent(other.transform);
                Dialogue_Menu.name = "Dialogue_Menu";

                LoadAllNpcDialogue(other);
                SetUIDialogue(TextNumber);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        DistoryNpcDialogue();
    }

    private void LoadAllNpcDialogue(Collider other) {
        NPCDialogue.NameOfNPC = other.name;
        int i = 0;
        foreach (string file in System.IO.Directory.GetFiles("Assets/My_Assets/My_Dialogue/" + NPCDialogue.NameOfNPC)){
            if (file.Remove(0, file.Length - 4) == ".txt") {
                string tempFileName = file.Remove(0, 29 + NPCDialogue.NameOfNPC.Length + 1);
                tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                NPCDialogue.FileName.Add(tempFileName);
                NPCDialogue.sentences.Add(LoadText.ReadText(NPCDialogue.NameOfNPC, tempFileName));
                i++;
                NPCDialogue.NumberOfSentences = i;
            }
        }
        Debug.Log("[" + NPCDialogue.NumberOfSentences + "] on Load");
    }

    private void SetUIDialogue(int Number) {
        GameObject.Find("Dialogue_Menu").transform.Find("Dialogue_Box").transform.Find("NPCName").GetComponent<Text>().text = NPCDialogue.NameOfNPC;
        GameObject.Find("Dialogue_Menu").transform.Find("Dialogue_Box").transform.Find("NPCDialogue").GetComponent<Text>().text = NPCDialogue.sentences[Number];
    }

    private void DistoryNpcDialogue() {
        Destroy(GameObject.Find("Dialogue_Menu").gameObject);
        NPCDialogue.OnDestroy();
        MenuOpen = false;
        TextNumber = 0;
    }

    public void ContinueDialogue() {
        DialogueManager Temp = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
        if (Temp.TextNumber < Temp.NPCDialogue.NumberOfSentences - 1) {
            Temp.TextNumber += 1;
            Temp.SetUIDialogue(Temp.TextNumber);
        }
        else { Temp.DistoryNpcDialogue(); }
    }
}
