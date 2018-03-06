using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerDll;
using Sirenix.OdinInspector;

public class Client_Manager : SerializedMonoBehaviour {
    
    private ServerDll.Client client = new ServerDll.Client();
    
    public bool ClientOn = false;

    public string ClientMessage = "Test";
    
    [Button]
    private void ConnectToServer() {
        if (client.TCP_ConnectToServer()) { ClientOn = true; }
        else { Debug.Log("TCP Failed"); }
    }
    [Button]
    private void Send() {
        client.TCP_SendData(ClientMessage);
    }

    void Start() {
    }


    // Update is called once per frame
    void Update () {
        if (ClientOn) {
            string TCP_Data = client.TCP_GetData();
            if (!string.IsNullOrEmpty(TCP_Data)) { Debug.Log(TCP_Data); }
        }
	}
    
}
