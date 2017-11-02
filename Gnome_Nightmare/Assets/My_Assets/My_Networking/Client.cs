using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class Client : MonoBehaviour {

    public string cientName;
    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public void ConnectToServer() {
        if (socketReady) { return; }

        string host = "127.0.0.1";
        int port = 4321;

        string h;
        int p;
        h = GameObject.Find("IPInput").GetComponent<InputField>().text;
        if (h != "") { host = h; }

        int.TryParse(GameObject.Find("IPInput").GetComponent<InputField>().text, out p);
        if (p != 0) { port = p; }

        try {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream); ;
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e) { Debug.Log("Socket Error: " + e.Message); }
    }

    private void Update() {
        if (socketReady) {
            if (stream.DataAvailable) {
                string data = reader.ReadLine();
                if (data != null) { OnIncomingData(data); }
            }
        }
    }

    private void OnIncomingData(string data){
        Debug.Log("[" + data + "]");
    }
}
