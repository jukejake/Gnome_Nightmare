using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerDll;
using Sirenix.OdinInspector;

public class Server_Manager : SerializedMonoBehaviour {

    private ServerDll.Server server = new ServerDll.Server();

    public bool ServerOn = false;

    public string ClientMessage = "Test";

    [Button]
    private void ServerStart() {
        if (server.TCP_Start()) { ServerOn = true; }
    }
    [Button]
    private void Send() {
        //server.(ClientMessage);
    }

    void Start() {
        //ServerStart();
    }


    // Update is called once per frame
    void Update () {
        if (ServerOn) {
            server.TCP_Update();
        }
	}
    

}
