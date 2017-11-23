using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class Client : MonoBehaviour {
    public GameObject chatContainer;
    public GameObject messagePrefab;

    public string clientName;
    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public void ConnectToServer() {
        if (socketReady) { return; }

        string host = "127.0.0.1";
        int port = 4321;
        string name = "Player";

        string h;
        int p;
        string n;
        h = GameObject.Find("IPInput").GetComponent<InputField>().text;
        if (h != "") { host = h; }

        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0) { port = p; }

        n = GameObject.Find("IDInput").GetComponent<InputField>().text;
        if (n != "") { name = n; }
        clientName = name;

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
        if (data == "%NAME") {
            Send("&NAME|" + clientName);
            return;
        }
        chatContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContainer.GetComponent<RectTransform>().sizeDelta.x, chatContainer.GetComponent<RectTransform>().sizeDelta.y + 30.0f);

        GameObject g = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
        g.GetComponentInChildren<Text>().text = data;

        Debug.Log("[" + data + "]");
    }
    private void Send(string data) {
        if (data.Contains("?Heal")) {
            PlayerManager.instance.GetComponent<PlayerStats>().FullHealth();
            return;
        }

        if (!socketReady) { return; }
        else {
            writer.WriteLine(data);
            writer.Flush();
        }
    }
    public void OnSendButton() {
        InputField input = GameObject.Find("ChatInput").GetComponent<InputField>();
        if (!string.IsNullOrEmpty(input.text)) {
            Send(input.text);
            input.text = "";
        }
    }

    private void CloseSocket() {
        if (!socketReady) { return; }

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
    private void OnApplicationQuit() {
        CloseSocket();
    }
    private void OnDisable() {
        CloseSocket();
    }
}
