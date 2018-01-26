using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Client_Manager : MonoBehaviour {

    [DllImport("Client")]
    public static extern void InitAccess();
    [DllImport("Client")]
    public static extern void DestroyManager();
    [DllImport("Client")]
    public static extern bool Initialize();
    [DllImport("Client")]
    public static extern void UpdateClient();
    [DllImport("Client")]
    public static extern IntPtr GetServerIP();
    [DllImport("Client")]
    public static extern bool CheckServerIP(string s_IP); //char * data
    [DllImport("Client")]
    public static extern bool SendData(string data); //const char * data
    [DllImport("Client")]
    public static extern IntPtr ReceiveData();


    private bool Initialized = false;
    private bool Connected = false;
    public string IP = "192.168.0.1";
    public string SentMessage = "Hello!";
    public string ReceivedMessage = "";

    // Use this for initialization
    void Start () {
        InitAccess();
        if (Initialize()) {
            Initialized = true;
            if (CheckServerIP(IP)) {
                Connected = true;
                SendData(SentMessage);
                ReceivedMessage = Marshal.PtrToStringAnsi(ReceiveData());
                Debug.Log(ReceivedMessage);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Initialized && !Connected) { CheckServerIP(IP); }
        else {
            //SendData(SentMessage);
            //ReceivedMessage = Marshal.PtrToStringAnsi(ReceiveData());
        }
	}
}
