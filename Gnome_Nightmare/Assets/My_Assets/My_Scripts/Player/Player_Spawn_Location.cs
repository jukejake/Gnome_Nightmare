using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random_Utils;

public class Player_Spawn_Location : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;

    // Use this for initialization
    void Start () {
        //Spawn the player
        GameObject player = (GameObject)Instantiate(PlayerPrefab, this.transform.position, this.transform.rotation);
        player.name = "Player";
        if (player.GetComponent<Agent>()) {
            if (Server_Manager.instance) { player.GetComponent<Agent>().AgentNumber = Server_Manager.instance.PlayerNumber; }
            else if (Client_Manager.instance) { player.GetComponent<Agent>().AgentNumber = Client_Manager.instance.PlayerNumber; }
            //Just in case //Though this will still case problems
            if (player.GetComponent<Agent>().AgentNumber == -1) { player.GetComponent<Agent>().AgentNumber = RandomUtils.RandomInt(2, 99); }

            player.GetComponent<Agent>().SendInstantiate();
        }
        //Spawn the Camera
        GameObject camera = (GameObject)Instantiate(CameraPrefab, this.transform.position, this.transform.rotation);
        camera.name = "Main Camera";
        camera.GetComponent<CameraFollow>().FollowThis = player.transform.Find("HitBox").Find("Top").Find("Head").gameObject;
        camera.GetComponent<CameraFollow>().endPoint = player.transform.Find("Endpoint").transform;
        
    }
}
