using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerDll;
using Sirenix.OdinInspector;

public class Client_Manager : SerializedMonoBehaviour {

    public static Client_Manager instance;
    private ID_Table IDTable;
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start() {
        IDTable = ID_Table.instance;
    }

    public int PlayerNumber = -1;


    private ServerDll.Client client = new ServerDll.Client();
    public bool ClientOn = false;
    public string ClientMessage = "Test";
    
    [Button]
    public void ConnectToServer() {
        if (client.TCP_ConnectToServer()) { ClientOn = true; }
    }
    [Button]
    public void Send() {
        client.TCP_SendData(ClientMessage);
    }
    public void SendData(string data) {
        client.TCP_SendData(data);
    }



    // Update is called once per frame
    private void Update () {
        if (IDTable == null) { IDTable = ID_Table.instance; }

        if (ClientOn) {
            string TCP_Data = client.TCP_GetData();
            if (!string.IsNullOrEmpty(TCP_Data)) {
                //Debug.Log(TCP_Data);
                ProcessData(TCP_Data);
            }
        }
	}

    private void ProcessData(string data) {
        bool destroy = false;
        int instantiate = -1;
        int ID = -1;
        Vector3 pos = new Vector3(); ;
        Vector3 rot = new Vector3(); ;
        int hp = -1;

        //Set Player Number
        if (data.Contains("!")) {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            PlayerNumber = int.Parse(t);

            data = data.Substring(t.Length + 2);
        }
        //Destroy an object
        if (data.Contains("~")) {
            destroy = true;
            data = data.Substring(1);
        }
        //Instantiate an object
        if (data.Contains("@")) {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            instantiate = int.Parse(t);
            data = data.Substring(t.Length+2);
        }
        //ID of an object
        if (data.Contains("#"))  {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            ID = int.Parse(t);
            data = data.Substring(t.Length+2);
        }
        //Position of an object
        if (data.Contains("&POS"))  {
            string t = data.Split('(')[1];
            t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            pos = new Vector3(x,y,z);
            data = data.Substring(t.Length+6);
        }
        //Rotation of an object
        if (data.Contains("&ROT"))  {
            string t = data.Split('(')[1];
            t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            rot = new Vector3(x,y,z);
            data = data.Substring(t.Length+6);
        }
        //Health of an object
        if (data.Contains("&HP")) {
            string t = data.Split('|')[0];
            t = t.Substring(3);
            hp = int.Parse(t);
            data = data.Substring(t.Length + 4);
        }

        //Need to handle Events
        if (ID == -1) { return; }

        if (destroy) {
            Debug.Log("Destroy: " + ID);
            //Find Object with the same [ID] and Destroy it.
            Agent[] agents = (Agent[]) GameObject.FindObjectsOfType(typeof(Agent));
            foreach (var agent in agents) {
                if (agent.AgentNumber == ID) {
                    Debug.Log("Destroyed: " + agent.gameObject.name + " | ID: " + ID);
                    Destroy(agent.gameObject);
                }
            }
        }
        else if (instantiate != -1) {
            //Instantiate and Object with [ID] from the ID_Table
            Debug.Log("Instantiate: " + instantiate + " | ID: " + ID);
            GameObject t;
            if (IDTable.IDTable.TryGetValue(instantiate, out t)) {
                GameObject temp = Instantiate(t, pos, Quaternion.Euler(rot));
                temp.name = t.name;
                if (temp.GetComponent<Agent>()) { temp.GetComponent<Agent>().AgentNumber = ID; }
                else {
                    temp.AddComponent<Agent>();
                    temp.GetComponent<Agent>().AgentNumber = ID;
                }
                if (hp != -1) {
                    if (temp.GetComponent<EnemyStats>())  { temp.GetComponent<EnemyStats>().MaxHealth = hp;  temp.GetComponent<EnemyStats>().CurrentHealth = hp; }
                    if (temp.GetComponent<PlayerStats>()) { temp.GetComponent<PlayerStats>().MaxHealth = hp; temp.GetComponent<PlayerStats>().CurrentHealth = hp; }
                }
                if (temp.tag == "Items") { temp.transform.SetParent(GameObject.FindGameObjectWithTag("Items_Spawn_Here").transform); }
                else if (temp.tag == "Enemy") { temp.transform.SetParent(GameObject.FindGameObjectWithTag("Enemies_Spawn_Here").transform); }
            }
        }
        else {
            //Find Object with the same [ID] and change it.
            Agent[] agents = (Agent[]) GameObject.FindObjectsOfType(typeof(Agent));
            foreach (var agent in agents) {
                if (agent.AgentNumber == ID) {
                    agent.gameObject.transform.position = pos;
                    agent.gameObject.transform.rotation = Quaternion.Euler(rot);
                    if (hp != -1 && agent.GetComponentInParent<EnemyStats>())  { agent.gameObject.GetComponent<EnemyStats>().CurrentHealth = hp; }
                    if (hp != -1 && agent.GetComponentInParent<PlayerStats>()) { agent.gameObject.GetComponent<PlayerStats>().CurrentHealth = hp; }
                    Debug.Log("Position: (" + pos.x + "," + pos.y + "," + pos.z + ")");
                    Debug.Log("Rotation: (" + rot.x + "," + rot.y + "," + rot.z + ")");
                    if (ID != -1 && hp >= 0) { Debug.Log("Health: " + hp); }
                }
            }
        }

    }

    public void Quit() {
        client.TCP_CloseSocket();
        Destroy(this);
    }


}
