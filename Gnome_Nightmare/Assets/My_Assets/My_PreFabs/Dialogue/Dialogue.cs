using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //[System.Serializable]
    //public class DialogueComponent {
    //    public string NameOfNPC;
    //    public string FileName;
    //    [TextArea(0, 100)]
    //    public List<string> sentences;
    //    public int NumberOfSentences;
    //}
    //public DialogueComponent[] NPCComps;


    public int NumberOfSentences;
    public string NameOfNPC;
    public List<string> FileName;
    //[TextArea(0, 100)]
    public List<string> sentences;

    public void OnDestroy() {
        NumberOfSentences = 0;
        NameOfNPC = string.Empty;
        FileName.Clear();
        sentences.Clear();
    }
}

