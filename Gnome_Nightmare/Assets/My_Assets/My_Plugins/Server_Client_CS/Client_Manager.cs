using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerDll;
using Sirenix.OdinInspector;

public class Client_Manager : SerializedMonoBehaviour {
    public int PlayerNumber = -1;
    void Awake() { DontDestroyOnLoad(this.gameObject); }
    private void Start() { }


    private ServerDll.Client client = new ServerDll.Client();
    public bool ClientOn = false;
    public string ClientMessage = "Test";
    
    [Button]
    public void ConnectToServer() {
        if (client.TCP_ConnectToServer()) { ClientOn = true; }
        else { Debug.Log("TCP Failed"); }
    }
    [Button]
    public void Send() {
        client.TCP_SendData(ClientMessage);
    }



    // Update is called once per frame
    private void Update () {
        if (ClientOn) {
            string TCP_Data = client.TCP_GetData();
            if (!string.IsNullOrEmpty(TCP_Data)) {
                //Debug.Log(TCP_Data);
                ProcessData(TCP_Data);
            }
        }
	}

    private void ProcessData(string data) {
        bool destroy = false;
        int instantiate = -1;
        int ID = -1;
        Vector3 pos = new Vector3(); ;
        Vector3 rot = new Vector3(); ;
        int hp = -1;

        //Set Player Number
        if (data.Contains("!")) {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            PlayerNumber = int.Parse(t);

            data = data.Substring(t.Length + 2);
        }
        //Destroy an object
        if (data.Contains("~")) {
            //Distroy Code Here
            destroy = true;

            data = data.Substring(1);
        }
        //Instantiate an object
        if (data.Contains("@")) {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            instantiate = int.Parse(t);
            //Instantiate Code Here

            data = data.Substring(t.Length+2);
        }
        //ID of an object
        if (data.Contains("#"))  {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            ID = int.Parse(t);
            data = data.Substring(t.Length+2);
        }
        //Position of an object
        if (data.Contains("&POS"))  {
            string t = data.Split('(')[1];
            t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            pos = new Vector3(x,y,z);
            data = data.Substring(t.Length+6);
        }
        //Rotation of an object
        if (data.Contains("&ROT"))  {
            string t = data.Split('(')[1];
            t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            rot = new Vector3(x,y,z);
            data = data.Substring(t.Length+6);
        }
        //Health of an object
        if (data.Contains("&HP")) {
            string t = data.Split('|')[0];
            t = t.Substring(3);
            hp = int.Parse(t);
            data = data.Substring(t.Length + 4);
        }

        if (destroy) { Debug.Log("Destroy: " + ID); }
        if (instantiate != -1) { Debug.Log("Instantiate: " + instantiate + " | ID: " + ID); }
        if (ID != -1 && pos != null) { Debug.Log("Position: (" + pos.x + "," + pos.y + "," + pos.z + ")"); }
        if (ID != -1 && rot != null) { Debug.Log("Rotation: (" + rot.x + "," + rot.y + "," + rot.z + ")"); }
        if (ID != -1 && hp >= 0) { Debug.Log("Health: " + hp); }
    }

    public void Quit() {
        client.TCP_CloseSocket();
        Destroy(this);
    }


}
