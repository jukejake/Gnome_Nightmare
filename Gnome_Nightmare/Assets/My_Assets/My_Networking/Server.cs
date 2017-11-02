using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {
    public int port = 4321;
    private TcpListener server;
    private bool serverStarted = false;
    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    // Use this for initialization
    private void Start () {
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
            StartListening();
            Debug.Log("Server has been started on port " + port.ToString());
        } catch (Exception e) { Debug.Log("Socket Error" + e.Message); }
    }

    private void Update() {
        if (!serverStarted) { return; }
        foreach (ServerClient c in clients) {
            if (!IsConnected(c.tcp)) {
                c.tcp.Close();
                disconnectList.Add(c);
                continue; //don't have to do this
            } else {
                Debug.Log(c.ClientName + " has connected");
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable) {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    if (data != null) { OnIncomingData(c, data); }
                }
            }
        }
    }

    private void OnIncomingData(ServerClient c, string data) {
        Debug.Log(c.ClientName + " sent [" + data + "]");
    }

    private bool IsConnected(TcpClient c) {
        try {
            if (c != null && c.Client != null && c.Client.Connected) {
                if (c.Client.Poll(0, SelectMode.SelectRead)) {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            else { return false; }
        }
        catch { return false; }
        
    }

    private void StartListening() {
        server.BeginAcceptSocket(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar) {
        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
    }
}

public class ServerClient {
    public TcpClient tcp;
    public string ClientName;

    public ServerClient(TcpClient clientSocket) {
        ClientName = "Guest";
        tcp = clientSocket;
    }
}