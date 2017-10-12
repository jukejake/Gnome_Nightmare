﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies_Movement : MonoBehaviour {

    [SerializeField]
    Transform destination;
    NavMeshAgent navMeshAgent;
    GameObject[] players;

    public float MaxFollowDistance = 1000.0f;
    public float timerDelay = 0.50f;
    private float timer = 0.0f;

    // Use this for initialization
    void Start () {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        players = GameObject.FindGameObjectsWithTag("Player");
    }


    private void Update() {
        //Will only update enemies seeking position at certain intervals
        if (timer > 0.0f) { timer -= Time.deltaTime; }
        else {
            timer = timerDelay;
            if (navMeshAgent == null) { return; }
            else { FindDestination(); }
        }
    }

    private void FindDestination() {
        //Makes sure there is players to seek
        if (players != null) {
            int playerNum = 0;
            float checkDistance = MaxFollowDistance;
            //Goes through all players and checks their distance to the enemy
            for (int i = 0; i < players.Length; i++) {
                //Distance between player and enemy
                float tempDistance = Vector3.Distance(players[i].transform.position, this.transform.position);
                //Finds closes player
                if (tempDistance <= checkDistance) {
                    checkDistance = tempDistance;
                    playerNum = i;
                }
            }
            //If the player is withen MaxFollowDistance
            if (checkDistance < MaxFollowDistance) {
                destination = players[playerNum].transform;
                SetDestination();
            }
            //The enemy will wonder
            else { Wonder(); }
        }
        else { return; }
    }

    private void SetDestination() {
        //If there is a destination set the navMeshAgent to go to it
        if (destination != null) {
            navMeshAgent.SetDestination(destination.transform.position);
        }
    }

    private void Wonder() {

    }

}
