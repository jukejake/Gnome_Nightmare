using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Sever_Manager : MonoBehaviour {


    [DllImport("Server")]
    public static extern void InitAccess();
    [DllImport("Server")]
    public static extern void DestroyManager();
    [DllImport("Server")]
    public static extern bool Initialize();
    [DllImport("Server")]
    public static extern void UpdateServer();
    [DllImport("Server")]
    public static extern bool SetPortNumber(int port);
    [DllImport("Server")]
    public static extern bool LookForActivity();
    [DllImport("Server")]
    public static extern bool NewConnection();
    [DllImport("Server")]
    public static extern IntPtr ReceiveData();
    [DllImport("Server")]
    public static extern IntPtr ReturnLastAddress();

    //private
    public bool Initialized = false;
    public bool PortSet = false;
    public int Port = 8888;
    public string SentMessage = "Hello!";
    public string ReceivedMessage = "";

    // Use this for initialization
    void Start () {
        DestroyManager();
        InitAccess();
        if (Initialize()) { Initialized = true; Debug.Log("What"); }
        //if (SetPortNumber(Port)) { PortSet = true; }

    }

    // Update is called once per frame
    void Update () {
        if (Initialized && LookForActivity()) {
            Debug.Log("...");
            if (NewConnection()) { Debug.Log("[Client has Connected]"); }
            //ReceivedMessage = Marshal.PtrToStringAnsi(ReceiveData());
            //if (ReceivedMessage != "") { Debug.Log(ReceivedMessage); }
        }
	}
}
