using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Network_Enemies_Movement : NetworkBehaviour {

    [SerializeField]
    Transform destination;
    NavMeshAgent navMeshAgent;
    GameObject[] players;

    public float MaxFollowDistance = 100.0f;
    public float MaxAttackDistance = 1.0f;
    public float timerDelay = 0.50f;
    private float timer = 0.0f;

    // Use this for initialization
    public void Start() {
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
            if (navMeshAgent == null) { }
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
                if (tempDistance <= checkDistance && !players[i].GetComponent<PlayerStats>().isDead) {
                    checkDistance = tempDistance;
                    playerNum = i;
                }
            }
            if (checkDistance < MaxAttackDistance) {
                AttackPlayer(playerNum);
                return;
            }
            //If the player is withen MaxFollowDistance
            else if (checkDistance < MaxFollowDistance) {
                destination = players[playerNum].transform;
                SetDestination();
                return;
            }
            //The enemy will wonder
            else { Wonder(); return; }
        }
        else { Debug.Log("No Players"); return; }
    }

    private void SetDestination() {
        //If there is a destination set the navMeshAgent to go to it
        if (destination != null) {
            navMeshAgent.SetDestination(destination.transform.position);
        }
    }

    private void AttackPlayer(int playerNum) {
        players[playerNum].GetComponent<PlayerStats>().TakeDamage(this.gameObject.GetComponent<EnemyStats>().Damage.GetValue());
    }

    private void Wonder() {
        timer = 2.0f;
        navMeshAgent.SetDestination(this.transform.position + RandomUtils.RandomVector3InBox(new Vector3(-10.0f, 1.0f, -10.0f),new Vector3(10.0f, 1.0f, 10.0f)));
    }

}
