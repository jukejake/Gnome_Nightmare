using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChatSystem;
using Sirenix.OdinInspector;

public class Chat_Manager : SerializedMonoBehaviour {

    public GameObject ServerLoginPrefab;

    [ToggleGroup("IsServer", order: 1, groupTitle: "Server")]
    public bool IsServer;
    [ToggleGroup("IsServer")]
    private ChatSystem.Server server = new ChatSystem.Server();

    [ToggleGroup("IsClient", order: 1, groupTitle: "Client")]
    public bool IsClient;
    [ToggleGroup("IsClient")]
    private ChatSystem.Client client = new ChatSystem.Client();
    [ToggleGroup("IsClient")]
    public GameObject chatContainer;
    [ToggleGroup("IsClient")]
    public GameObject messagePrefab;




    // Use this for initialization
    public void Start () {
        //GameObject Login = Instantiate<GameObject>(ServerLoginPrefab);
        //Login.name = ServerLoginPrefab.name;

        if (IsServer) { ServerStart(); }
        if (IsClient) { ClientStart(); }
    }

    // Update is called once per frame
    public void Update () {
        if (IsServer) { ServerUpdate(); }
        if (IsClient) { ClientUpdate(); }
    }



    [ToggleGroup("IsServer")]
    private void ServerStart() {
        server.Start();
    }
    [ToggleGroup("IsServer")]
    private void ServerUpdate() {
        server.Update();
    }



    [ToggleGroup("IsClient")]
    private void ClientStart() {
        client.chatContainer = chatContainer;
        client.messagePrefab = messagePrefab;
    }
    [ToggleGroup("IsClient")]
    private void ClientUpdate() {
        //if (chatContainer != null) { chatContainer = GameObject.Find("Content").gameObject; }
        if (chatContainer != null) { client.chatContainer = chatContainer; }
        if (messagePrefab != null) { client.messagePrefab = messagePrefab; }
        //client.Update();

        string temp = client.InstantiateData();
        if (temp != null && temp != "") {
            GameObject g = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
            g.GetComponentInChildren<Text>().text = temp;
            Debug.Log("[" + temp + "]");
        }
        
    }
    [ToggleGroup("IsClient")]
    public void ConnectToServer() {
        client.ConnectToServer();
    }
    [ToggleGroup("IsClient")]
    public void OnSendButton() {
        client.OnSendButton();
    }
    [ToggleGroup("IsClient")]
    public void OnApplicationQuit() {
        client.OnApplicationQuit();
    }
}

