using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies_Movement : MonoBehaviour {

    [SerializeField]
    Transform destination;
    NavMeshAgent navMeshAgent;
    GameObject[] players;

    float timer = 0.0f;

	// Use this for initialization
	void Start () {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        Invoke("DelayedStart", 0.1f);
    }

    private void DelayedStart() {
        players = GameObject.FindGameObjectsWithTag("Player");
    }


    private void Update() {
        if (timer > 0.0f) { timer -= Time.deltaTime; }
        else {
            timer = 0.5f;
            if (navMeshAgent == null) { return; }
            else { FindDestination(); }
        }
    }

    private void FindDestination() {
        if (players != null) {
            int playerNum = 0;
            float checkDistance = 1000;
            for (int i = 0; i < players.Length; i++) {
                float tempDistance = Vector3.Distance(players[i].transform.position, this.transform.position);
                if (tempDistance <= checkDistance) {
                    checkDistance = tempDistance;
                    playerNum = i;
                }
            }
            destination = players[playerNum].transform;
            SetDestination();
        }
        else { return; }
    }

    private void SetDestination() {
        if (destination != null) {
            navMeshAgent.SetDestination(destination.transform.position);
        }
    }

}
