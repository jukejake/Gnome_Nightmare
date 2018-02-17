using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Sirenix.OdinInspector;

public class Client_Manager : SerializedMonoBehaviour {

    [DllImport("Client")]
    public static extern void InitAccess();
    [DllImport("Client")]
    public static extern void DestroyManager();

    [DllImport("Client")]
    public static extern void SetPortNumber(int port);
    [DllImport("Client")]
    public static extern void SetIPNumber(string ip);
    [DllImport("Client")]
    public static extern void SendData(string data);
    [DllImport("Client")]
    public static extern void SetState(bool state);
    [DllImport("Client")]
    public static extern bool GetState();

    [DllImport("Client")]
    public static extern bool Initialize();
    [DllImport("Client")]
    public static extern void Shutdown();

    [DllImport("Client")]
    public static extern IntPtr Unity_TCPReceive();
    [DllImport("Client")]
    public static extern IntPtr Unity_UDPReceive();
    [DllImport("Client")]
    public static extern bool Unity_Initialize();
    [DllImport("Client")]
    public static extern void Unity_Shutdown();




    public bool isRunning = false;
    [HideIf("Initialized", true)]
    public string IP = "127.0.0.1";
    [HideIf("Initialized", true)]
    public int Port = 8888;

    [HideIf("Initialized", true), ReadOnly]
    public bool Initialized = false;
    [ShowIf("Initialized", true), HideIf("Run", true), ReadOnly]
    public bool Run = false;

    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public string SentMessage = "I'm a user.";
    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public List<string> TCPReceive_Message;
    [ShowIf("Initialized", true), ShowIf("Run", true)]
    public List<string> UDPReceive_Message;

    private Thread Thread_TCPReceive;
    private Thread Thread_UDPReceive;
    private AutoResetEvent ResetEvent_TCP;
    private AutoResetEvent ResetEvent_UDP;

    [Button, ShowIf("Initialized", true), ShowIf("Run", true)]
    public void SendToServer() { SendData(SentMessage); }

    [Button, HideIf("Run", true)]
    public void StartClient () {
        InitAccess();

        SetIPNumber(IP);
        SetPortNumber(Port);

        if (Unity_Initialize()) {
            Initialized = true;
            Run = true;
            Debug.Log("Initialized Client");

            Thread_TCPReceive = new Thread(Run_TCP); Thread_TCPReceive.Start();
            Thread_UDPReceive = new Thread(Run_UDP); Thread_UDPReceive.Start();

            Thread_TCPReceive.IsBackground = true;
            Thread_UDPReceive.IsBackground = true;

            ResetEvent_TCP = new AutoResetEvent(false);
            ResetEvent_UDP = new AutoResetEvent(false);
        }
        
    }

    [Button, ShowIf("Run", true)]
    public void CloseClient() { Clear(); }

    private void Run_TCP() {
        while (Run) {
            ResetEvent_TCP.WaitOne();

            string temp = "";
            temp = Marshal.PtrToStringAnsi(Unity_TCPReceive());
            if (temp != null && temp != "") { TCPReceive_Message.Add(temp); }

            //ResetEvent_UDP.WaitOne();
        }
    }
    private void Run_UDP() {
        while (Run) {
            ResetEvent_UDP.WaitOne();

            string temp = "";
            temp = Marshal.PtrToStringAnsi(Unity_UDPReceive());
            if (temp != null && temp != "") { UDPReceive_Message.Add(temp); }

            //ResetEvent_UDP.WaitOne();
        }
    }

    private void FixedUpdate() {
        if (Initialized) { isRunning = GetState(); }
        if (Run) { 
            ResetEvent_TCP.Set();
            ResetEvent_UDP.Set();
        }
    }



    private void Clear() { 
        if (Initialized) {
            Thread_TCPReceive.Abort();
            Thread_UDPReceive.Abort();
            Unity_Shutdown();
            Run = false;
            Initialized = false;

            TCPReceive_Message.Clear();
            UDPReceive_Message.Clear();
            SetState(false);
        }
    }
    private void OnApplicationQuit() { Clear(); }
    private void OnDestroy() { Clear(); }
}
