using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner_Manager : MonoBehaviour {


    public static Spawner_Manager instance;
    void Awake() { instance = this; }
    public GameObject[] spawners;

    private int CurrentLevel = 0;
    private int OldLevel = -1;

    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        ActivateAllSpawners();
    }

    private void Update() {
        UpdateUI();
    }

    public void UpdateUI() {
        if (GameObject.Find("World").transform.Find("Menu").transform.Find("Enemy Info")) {
            GameObject temp = GameObject.Find("World").transform.Find("Menu").transform.Find("Enemy Info").gameObject;
            temp.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave] [" + CheckAliveEnemyCount() + "/" + CheckTotalEnemyCount() + "-Enemies]");
        }
        if (OldLevel == CurrentLevel && CheckAliveEnemyCount() == 0) {
            for (int i = 0; i < spawners.Length; i++) {
                if (spawners[i].GetComponent<Wave_Spawners>()) {
                        spawners[i].GetComponent<Wave_Spawners>().AddStats("Amount", 1);
                }
            }
            ActivateAllSpawners();
        }
    }

    private int CheckAliveEnemyCount() {
        if (GameObject.Find("World").transform.Find("Enemies")) {
            return GameObject.Find("World").transform.Find("Enemies").childCount;
        }
        else { return 0; }
    }
    private int CheckTotalEnemyCount() {
        int EnemyCount = 0;
        for (int i = 0; i < spawners.Length; i++) {
            if (spawners[i].GetComponent<Wave_Spawners>()) {
                EnemyCount += spawners[i].GetComponent<Wave_Spawners>().TotalEnemyCount();
            }
        }
        return EnemyCount;
    }

    public void CheckAllSpawners() {
        int countDeActive = 0;
        for (int i = 0; i < spawners.Length; i++) {
            if (spawners[i].GetComponent<Wave_Spawners>()) {
                if (spawners[i].GetComponent<Wave_Spawners>().DefeatedSpawner) {
                    countDeActive += 1;
                }
            }
        }
        if (countDeActive == spawners.Length) { OldLevel = CurrentLevel; } //Debug.Log("All are DeActive");
        else if (countDeActive < spawners.Length) { }//Debug.Log((spawners.Length - countDeActive) + " are still Active");
        else { Debug.Log("Somehow there are " + (countDeActive - spawners.Length) + " more DeActive spawners than there are spawners"); }
    }

    public void ActivateAllSpawners() {
        CurrentLevel += 1;
        for (int i = 0; i < spawners.Length; i++) {
            if (spawners[i].GetComponent<Wave_Spawners>()) {
                if (spawners[i].GetComponent<Wave_Spawners>().DefeatedSpawner) {
                    spawners[i].GetComponent<Wave_Spawners>().DefeatedSpawner = false;
                }
            }
        }
    }
    public void DeActivateAllSpawners() {
        for (int i = 0; i < spawners.Length; i++) {
            if (spawners[i].GetComponent<Wave_Spawners>()) {
                if (spawners[i].GetComponent<Wave_Spawners>().DefeatedSpawner == false) {
                    spawners[i].GetComponent<Wave_Spawners>().DefeatedSpawner = true;
                }
            }
        }
    }


    public void SetAllIntervalBetweenSpawns(float amount) {
        for (int i = 0; i < spawners.Length; i++) {
            if (spawners[i].GetComponent<Wave_Spawners>()) {
                spawners[i].GetComponent<Wave_Spawners>().IntervalBetweenSpawns = amount;
            }
        }
    }
    public void SetAllIntervalBetweenRounds(float amount) {
        for (int i = 0; i < spawners.Length; i++) {
            if (spawners[i].GetComponent<Wave_Spawners>()) {
                spawners[i].GetComponent<Wave_Spawners>().IntervalBetweenRounds = amount;
            }
        }
    }

}
