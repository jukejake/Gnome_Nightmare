using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Spawn_Location : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;

    // Use this for initialization
    void Start () {
        GameObject player = (GameObject)Instantiate(PlayerPrefab, this.transform.position, this.transform.rotation);
        player.name = "Player";
        GameObject camera = (GameObject)Instantiate(CameraPrefab, this.transform.position, this.transform.rotation);
        camera.name = "Main Camera";
        camera.GetComponent<CameraFollow>().FollowThis = player.transform.Find("HitBox").Find("Head").gameObject;
        camera.GetComponent<CameraFollow>().endPoint = player.transform.Find("Endpoint").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
