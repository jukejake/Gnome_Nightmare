using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Sirenix.OdinInspector;

public class Sever_Manager : SerializedMonoBehaviour {

    [DllImport("Server")]
    public static extern void InitAccess();
    [DllImport("Server")]
    public static extern void DestroyManager();

    [DllImport("Server")]
    public static extern void SetPortNumber(int port);
    [DllImport("Server")]
    public static extern void SendData(string data);
    [DllImport("Server")]
    public static extern void SetState(bool state);
    [DllImport("Server")]
    public static extern bool GetState();

    [DllImport("Server")]
    public static extern bool Initialize();
    [DllImport("Server")]
    public static extern void Shutdown();

    [DllImport("Server")]
    public static extern IntPtr Unity_TCPListen();
    [DllImport("Server")]
    public static extern IntPtr Unity_TCPReceive();
    [DllImport("Server")]
    public static extern IntPtr Unity_UDPFunction();
    [DllImport("Server")]
    public static extern bool Unity_Initialize();
    [DllImport("Server")]
    public static extern void Unity_Shutdown();


    public bool isRunning = false;
    [HideIf("Initialized", true)]
    public int Port = 8888;

    [HideIf("Initialized", true), ReadOnly]
    public bool Initialized = false;
    [ShowIf("Initialized", true), HideIf("Run", true), ReadOnly]
    public bool Run = false;

    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public string SentMessage = "This is the server.";
    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public List<string> TCPListen_Message;
    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public List<string> TCPReceive_Message;
    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public List<string> UDPFunction_Message;

    private Thread Thread_TCP;
    //private Thread Thread_TCPListen;
    //private Thread Thread_TCPReceive;
    private Thread Thread_UDPFunction;
    private AutoResetEvent ResetEvent_TCP;
    //private AutoResetEvent ResetEvent_TCPL;
    //private AutoResetEvent ResetEvent_TCPR;
    private AutoResetEvent ResetEvent_UDP;


    [Button, ShowIf("Initialized", true), ShowIf("Run", true)]
    public void SendToClients() { SendData("broadcast " + SentMessage); }

    [Button, HideIf("Run", true)]
    public void StartServer() {
        InitAccess();

        SetPortNumber(Port);

        if (Unity_Initialize()) {
            Initialized = true;
            Run = true;
            Debug.Log("Initialized Server");


            Thread_TCP = new Thread(Run_TCP);
            Thread_TCP.IsBackground = true;
            Thread_TCP.Start();
            ResetEvent_TCP = new AutoResetEvent(false);

            //Thread_TCPListen = new Thread(Run_TCPL); 
            //Thread_TCPListen.IsBackground = true;
            //Thread_TCPListen.Start();
            //ResetEvent_TCPL = new AutoResetEvent(false);

            //Thread_TCPReceive = new Thread(Run_TCPR); 
            //Thread_TCPReceive.IsBackground = true;
            //Thread_TCPReceive.Start();
            //ResetEvent_TCPR = new AutoResetEvent(false);

            Thread_UDPFunction = new Thread(Run_UDP);
            Thread_UDPFunction.IsBackground = true;
            Thread_UDPFunction.Start();
            ResetEvent_UDP = new AutoResetEvent(false);


        }

    }

    [Button, ShowIf("Run", true)]
    public void CloseServer() { Clear(); }



    private void Run_TCP() {
        while (Run) {
            //ResetEvent_TCP.WaitOne();
            string temp = "";

            temp = Marshal.PtrToStringAnsi(Unity_TCPListen());
            if (temp != null && temp != "") { TCPListen_Message.Add(temp); }

            temp = "";
            temp = Marshal.PtrToStringAnsi(Unity_TCPReceive());
            if (temp != null && temp != "") { TCPReceive_Message.Add(temp); }

            ResetEvent_TCP.WaitOne();
        }
    }

    private void Run_TCPL() {
        while (Run) {
            //ResetEvent_TCPL.WaitOne();
            string temp = Marshal.PtrToStringAnsi(Unity_TCPListen());
            if (temp != null) { TCPListen_Message.Add(temp); }
            //ResetEvent_UDP.WaitOne();
        }
    }
    private void Run_TCPR() {
        while (Run) {
            //ResetEvent_TCPR.WaitOne();
            string temp = Marshal.PtrToStringAnsi(Unity_TCPReceive());
            if (temp != null) { TCPReceive_Message.Add(temp); }
            //ResetEvent_UDP.WaitOne();
        }
    }
    private void Run_UDP() {
        while (Run) {
            //ResetEvent_UDP.WaitOne();

            string temp = "";
            temp = Marshal.PtrToStringAnsi(Unity_UDPFunction());
            if (temp != null && temp != "") { UDPFunction_Message.Add(temp); }

            ResetEvent_UDP.WaitOne();
        }
    }


    private void FixedUpdate() {
        //
        //if (Initialized){ isRunning = GetState(); }
        if (Run) {
            ResetEvent_TCP.Set();
            //ResetEvent_TCPL.Set();
            //ResetEvent_TCPR.Set();
            ResetEvent_UDP.Set();
        }
    }




    private void Clear() {
        if (Initialized) {
            Thread_TCP.Abort();
            //Thread_TCPListen.Abort();
            //Thread_TCPReceive.Abort();
            Thread_UDPFunction.Abort();
            Unity_Shutdown();
            Run = false;
            Initialized = false;

            TCPListen_Message.Clear();
            TCPReceive_Message.Clear();
            UDPFunction_Message.Clear();
            SetState(false);
        }
    }
    private void OnApplicationQuit() { Clear(); }
    private void OnDestroy() { Clear(); }
}
