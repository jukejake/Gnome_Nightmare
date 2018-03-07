using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerDll;
using Sirenix.OdinInspector;

public class Server_Manager : SerializedMonoBehaviour {
    private void Awake() { DontDestroyOnLoad(this.gameObject); }
    private void Start() { ServerStart(); }

    private ServerDll.Server server = new ServerDll.Server();
    public bool ServerOn = false;
    public string ServerMessage = "~@456|#123|&POS(1.2,3.4,5.6)&ROT(9.8,7.6,5.4)&HP26|&Hello";

    [Button]
    private void ServerStart() {
        if (server.TCP_Start()) { ServerOn = true; }
    }
    [Button]
    private void Send() {
        server.TCP_Send(ServerMessage);
    }


    // Update is called once per frame
    private void Update () {
        if (ServerOn) {
            server.TCP_UpdateV2();
            foreach (ServerClient c in server.TCP_Clients) { 
                string TCP_Data = server.TCP_GetDataV2(c);
                if (!string.IsNullOrEmpty(TCP_Data)) {
                    //Debug.Log(TCP_Data);
                    ProcessData(TCP_Data);
                }
            }
        }
	}
    
    private void ProcessData(string data) {
        bool destroy = false;
        int instantiate = -1;
        int ID = -1;
        Vector3 pos = new Vector3();
        Vector3 rot = new Vector3();
        int hp = -1;


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
        server.TCP_Close();
        Destroy(this);
    }

}
