using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class Spawner_Manager : SerializedMonoBehaviour {


    public static Spawner_Manager instance;
    void Awake() { instance = this; }

    [TableList]
    public List<Spawners> SpawnersTable = new List<Spawners>();

    public float TimeBetweenRounds = 10.0f;
    private int CurrentLevel = 0;
    private int OldLevel = -1;

    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        ActivateAllSpawnersInCurrentRound();
    }

    private void Update() {
        UpdateUI();
    }

    //Updates the UI and re-activates spawners at the end of the round
    public void UpdateUI() {
        //Finds and updates the UI
        if (GameObject.Find("World").transform.Find("Menu").transform.Find("Enemy Info")) {
            GameObject temp = GameObject.Find("World").transform.Find("Menu").transform.Find("Enemy Info").gameObject;
            temp.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave] [" + CheckAliveEnemyCount() + "/" + CheckTotalEnemyCount() + "-Enemies]");
        }
        //At the end of a round spawn a new round
        if (OldLevel == CurrentLevel && CheckAliveEnemyCount() == 0) {
            //Add an enemy to all the spawners in previous round
            AddEnemyToAllSpawnersInCurrentRound(CurrentLevel-1, 1);
            //Activate all spawners
            ActivateAllSpawnersInCurrentRound();
        }
    }

    //Checks the amount of enemies currently alive
    private int CheckAliveEnemyCount() {
        //Finds where the enemies are spawned to and returns the amount of childern (ie. number of enemies)
        if (GameObject.Find("World").transform.Find("Enemies")) {
            return GameObject.Find("World").transform.Find("Enemies").childCount;
        }
        else { return 0; }
    }

    //Checks the total amount of enemies that will spawn per round
    private int CheckTotalEnemyCount() {
        int EnemyCount = 0;
        //Goes through each spawner and counts The amount of enemies it will spawn
        for (int i = 0; i < SpawnersTable.Count; i++) {
            //If spawner is not active or it does not have the component than return
            if (SpawnersTable[i].Toggle || !SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) { }
            else if ((CurrentLevel > SpawnersTable[i].StartAt && CurrentLevel == (SpawnersTable[i].LastAvtiveRound + SpawnersTable[i].ActiveEvery)) || (CurrentLevel == SpawnersTable[i].StartAt)) {
                //counts amount of enemies to spawn multiplyed by number of waves in spawner
                EnemyCount += (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().TotalEnemyCount() * SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().NumberOfWaves);
            }
        }
        return EnemyCount;
    }

    //Checks if all spawners are DeActive
    public void CheckAllSpawners() {
        int countDeActive = 0;
        //Goes through each spawner and checks if they are de-active
        for (int i = 0; i < SpawnersTable.Count; i++) {
            //If spawner does not have the component than return
            if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) {
                if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().DefeatedSpawner) {
                    countDeActive += 1;
                }
            }
        }
        //If all spawners are de-active than Allow it to go to the next level (ie. if(OldLevel == CurrentLevel){ActivateAllSpawners();})
        if (countDeActive == SpawnersTable.Count) { OldLevel = CurrentLevel; } //Debug.Log("All are DeActive");
        //If some spawners are still active
        else if (countDeActive < SpawnersTable.Count) { }//Debug.Log((spawners.Length - countDeActive) + " are still Active");
        //um... Somehow there are more de-active spawners than there are spawners
        else { Debug.Log("Somehow there are " + (countDeActive - SpawnersTable.Count) + " more DeActive spawners than there are spawners."); }
    }

    //Activates all spawners in current round
    public void ActivateAllSpawnersInCurrentRound() {
        CurrentLevel += 1;
        //Goes through all spawners
        for (int i = 0; i < SpawnersTable.Count; i++) {
            //If spawner is not active or it does not have the component than return
            if (SpawnersTable[i].Toggle || !SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) { }
            else if ((CurrentLevel > SpawnersTable[i].StartAt && CurrentLevel == (SpawnersTable[i].LastAvtiveRound + SpawnersTable[i].ActiveEvery)) || (CurrentLevel == SpawnersTable[i].StartAt)) {
                SpawnersTable[i].LastAvtiveRound = CurrentLevel;
                //If the spawner is deactive then active it
                if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().DefeatedSpawner) {
                    SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().DefeatedSpawner = false;
                    SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().IntervalBetweenRounds = TimeBetweenRounds;
                    SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().spawnCoolDownRemaining = TimeBetweenRounds;
                }
            }
        }
    }

    //Sets all the spawners to Deactive
    public void DeActivateAllSpawners() {
        //Goes through all spawners
        for (int i = 0; i < SpawnersTable.Count; i++) {
            //If spawner does not have the component than return
            if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) {
                if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().DefeatedSpawner == false) {
                    SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().DefeatedSpawner = true;
                }
            }
        }
    }

    //Sets the amount of time between spawns
    public void SetAllIntervalBetweenSpawns(float amount) {
        //Goes through all spawners
        for (int i = 0; i < SpawnersTable.Count; i++) {
            if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) {
                SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().IntervalBetweenSpawns = amount;
            }
        }
    }
    //Sets the amount of time between rounds
    public void SetAllIntervalBetweenRounds(float amount) {
        //Goes through all spawners
        for (int i = 0; i < SpawnersTable.Count; i++) {
            if (SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) {
                SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().IntervalBetweenRounds = amount;
            }
        }
    }

    //Adds an enemy to all spawners in current round
    public void AddEnemyToAllSpawnersInCurrentRound(int RoundNumber, int AddAmount) {
        //Goes through all spawners
        for (int i = 0; i < SpawnersTable.Count; i++) {
            //If spawner is not active or it does not have the component than return
            if (SpawnersTable[i].Toggle || !SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>()) { }
            else if ((RoundNumber > SpawnersTable[i].StartAt && RoundNumber == (SpawnersTable[i].LastAvtiveRound + SpawnersTable[i].ActiveEvery)) || (RoundNumber == SpawnersTable[i].StartAt)) {
                //Add an extra enemy to all spawners (all waves in the spawners)
                SpawnersTable[i].Spawner.GetComponent<Wave_Spawners>().AddStats("Amount", AddAmount);
            }
        }
    }
}

public class Spawners {
    [TableColumnWidth(50)]
    public bool Toggle;

    public GameObject Spawner;

    [TableColumnWidth(50)]
    public int StartAt = 1;
    [TableColumnWidth(80)]
    public int ActiveEvery = 1;
    [HideInInspector]
    [TableColumnWidth(1)]
    public int LastAvtiveRound = 0;
}
