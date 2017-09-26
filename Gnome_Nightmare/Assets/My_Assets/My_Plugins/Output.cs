using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Output : MonoBehaviour {
    [DllImport("Helloworld")]
    public static extern IntPtr Helloworld();
    [DllImport("Helloworld")]
    public static extern void Log(string CharName, string ItemName, string Value);
    [DllImport("Helloworld")]
    public static extern void ClearLog(string CharName, string ItemName);
    [DllImport("Helloworld")]
    public static extern IntPtr LoadDialogue(string CharName, string ItemName);
    [DllImport("Helloworld")]
    public static extern void SaveDialogue(string CharName, string ItemName, string Value);


    [System.Serializable]
    public class TextComponent
    {
        public string Path;
        public string Object;
        public string Text;
    }
    public TextComponent[] textComps;

    // Use this for initialization
    void Start () {
        //Debug.Log(Marshal.PtrToStringAnsi(Helloworld()));
        ClearLog(gameObject.transform.name, gameObject.transform.ToString());
        //Debug.Log(Marshal.PtrToStringAnsi(LoadDialogue("NPC", "Hello")));
        SaveDialogue("NPC", "Talk_To_Me", "Talk to me! I have feelings too! JUST KILL MEEEEE!!!!!!");
        //Debug.Log(Marshal.PtrToStringAnsi(LoadDialogue("NPC", "Talk_To_Me")));
        //ClearLog("NPC", "Talk_To_Me");

    }
	
	// Update is called once per frame
	void Update () {
        Log(gameObject.transform.name, gameObject.transform.ToString(), gameObject.transform.position.ToString());
        foreach (TextComponent tc in textComps) {
            //Debug.Log(ReadText(tc.Path, tc.Object));
        }
    }

    public void SaveText() {
        foreach (TextComponent tc in textComps) {
             WriteText(tc.Path, tc.Object, tc.Text);
             tc.Text = ReadText(tc.Path, tc.Object);
        }
    }


    public void WriteText(string v_Path, string v_Object, string v_Text) {
        SaveDialogue(v_Path, v_Object, v_Text);
    }

    public string ReadText(string v_Path, string v_Object) {
        string v_Text = Marshal.PtrToStringAnsi(LoadDialogue(v_Path, v_Object));
        if (v_Text == null) { v_Text = "ERROR.003"; }

        //Debug.Log(Marshal.PtrToStringAnsi(LoadDialogue(v_Path, v_Object)));
        //Debug.Log(LoadDialogue(v_Path, v_Object));
        //Debug.Log(v_Text);

        return v_Text;
    }
}

[CustomEditor(typeof(Output))]
public class OutputEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Output myScript = (Output)target;
        if (GUILayout.Button("Save Text file")) {
            myScript.SaveText();
        }
    }
}