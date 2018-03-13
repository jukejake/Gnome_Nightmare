using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Agent : SerializedMonoBehaviour {

    [HorizontalGroup("Basic Info 1", 0.5f), LabelWidth(50)]
    public float StartIn = 1.0f;
    [HorizontalGroup("Basic Info 1", 0.5f), LabelWidth(90)]
    public float RepeatEvery = 1.0f;
    [HorizontalGroup("Basic Info 2", 0.5f), LabelWidth(90)]
    public int AgentNumber = -1;
    [HorizontalGroup("Basic Info 2", 0.5f), LabelWidth(90)]
    public int PrefabNumber = -1;
    private Server_Manager server;
    private Client_Manager client;
    private EnemyStats enemyHealth;
    private PlayerStats playerHealth;
    private void Awake() {
        if (Client_Manager.instance) { client = Client_Manager.instance; }
        else if (Server_Manager.instance) { server = Server_Manager.instance; }
    }
    private void Start() {
        // Repeat X, in Y seconds, every Z seconds.
        if (client) { InvokeRepeating("ClientRepeatThis", StartIn, RepeatEvery); }
        else if (server) { InvokeRepeating("ServerRepeatThis", StartIn, RepeatEvery); }

        if (this.GetComponent<EnemyStats>()) { enemyHealth = this.GetComponent<EnemyStats>(); }
        if (this.GetComponent<PlayerStats>()) { playerHealth = this.GetComponent<PlayerStats>(); }
    }


    [HorizontalGroup("Stuff To Send", 0.03f)]
    [BoxGroup("Stuff To Send/1", false), LabelWidth(50)]
    public bool Position = false;
    [BoxGroup("Stuff To Send/2", false), LabelWidth(50)]
    public bool Rotation = false;
    [BoxGroup("Stuff To Send/3", false), LabelWidth(40)]
    public bool Health = false;

    private Vector3 pos = new Vector3(0.0f,0.0f,0.0f);
    private Quaternion rot = new Quaternion(0.0f,0.0f,0.0f,0.0f);
    private float health = 0.0f;


    public void SendInstantiate() {

        if (PrefabNumber == -1 || AgentNumber == -1) { return; }

        string temp = ("@" + PrefabNumber.ToString() + "|" + "#" + AgentNumber.ToString() + "|");
        if (Position) {
            Vector3 pos = this.gameObject.transform.position;
            temp += ("&POS(" + pos.x.ToString() + "," + pos.y.ToString() + "," + pos.z.ToString() + ")");
        }
        if (Rotation) {
            Quaternion rot = this.gameObject.transform.rotation;
            temp += ("&POS(" + rot.x.ToString() + "," + rot.y.ToString() + "," + rot.z.ToString() + ")");
        }
        if (Health) {
            if (enemyHealth) { temp += ("@HP" + enemyHealth.CurrentHealth.ToString() + "|"); }
            if (playerHealth) { temp += ("@HP" + playerHealth.CurrentHealth.ToString() + "|"); }
        }
        if (client) { client.SendData(temp); }
        if (server) { server.SendData(temp); }
        
    }

    private void ClientRepeatThis() {

        if (AgentNumber == -1) { return; }

        Debug.Log("Client");

        string temp = ("#" + AgentNumber.ToString() + "|");
        if (Position) {
            if (pos != this.gameObject.transform.position) {
                pos = this.gameObject.transform.position;
                temp += ("&POS(" + pos.x.ToString() + "," + pos.y.ToString() + "," + pos.z.ToString() + ")");
            }
        }
        if (Rotation) {
            if (rot != this.gameObject.transform.rotation) {
                rot = this.gameObject.transform.rotation;
                temp += ("&POS(" + rot.x.ToString() + "," + rot.y.ToString() + "," + rot.z.ToString() + ")");
            }
        }
        if (Health) {
            if (enemyHealth && health != enemyHealth.CurrentHealth) { health = enemyHealth.CurrentHealth; temp += ("@HP" + enemyHealth.CurrentHealth.ToString() + "|"); }
            if (playerHealth && health != enemyHealth.CurrentHealth) { health = playerHealth.CurrentHealth; temp += ("@HP" + playerHealth.CurrentHealth.ToString() + "|"); }
        }
        if (temp != ("#" + AgentNumber.ToString() + "|")) { client.SendData(temp); }
        
    }
    private void ServerRepeatThis() {

        if (AgentNumber == -1) { return; }

        Debug.Log("Server");

        string temp = ("#" + AgentNumber.ToString() + "|");
        if (Position) {
            Vector3 pos = this.gameObject.transform.position;
            temp += ("&POS(" + pos.x.ToString() + "," + pos.y.ToString() + "," + pos.z.ToString() + ")");
        }
        if (Rotation) {
            Quaternion rot = this.gameObject.transform.rotation;
            temp += ("&POS(" + rot.x.ToString() + "," + rot.y.ToString() + "," + rot.z.ToString() + ")");
        }
        if (Health) {
            if (enemyHealth) { temp += ("@HP" + enemyHealth.CurrentHealth.ToString() + "|"); }
            if (playerHealth) { temp += ("@HP" + playerHealth.CurrentHealth.ToString() + "|"); }
        }
        if (temp != ("#" + AgentNumber.ToString() + "|")) { server.SendData(temp); }
    }

    public void SendDestroy() {

        if (AgentNumber == -1) { return; }

        string temp = ("~#" + AgentNumber.ToString() + "|");

        if (client) { client.SendData(temp); }
        if (server) { server.SendData(temp); }

    }

    private void OnDestroy() { SendDestroy(); }



}
