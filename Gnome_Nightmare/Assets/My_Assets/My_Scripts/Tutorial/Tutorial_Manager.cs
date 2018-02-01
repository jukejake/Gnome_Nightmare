using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using EnemySpawners;

public class Tutorial_Manager : SerializedMonoBehaviour {

    public bool On = true;
    public int Stage = -1;

    [BoxGroup(group:"Prefabs")]
    public GameObject HouseFog;
    private GameObject T_HouseFog;
    [BoxGroup(group: "Prefabs")]
    public GameObject BunkerFog;
    private GameObject T_BunkerFog;
    [BoxGroup(group: "Prefabs")]
    public GameObject Fire;
    private GameObject T_Fire;
    [BoxGroup(group: "Managers")]
    public Interface_SpawnTable SpawnManager;
    private int OldLevel = 0;
    [BoxGroup(group: "Barn Area")]
    public GameObject BarnArea;



    // Use this for initialization
    void Start () {
        if (On) {
            Stage = 0;
            T_HouseFog = (GameObject)Instantiate(HouseFog);
            T_HouseFog.name = HouseFog.name;
            T_BunkerFog = (GameObject)Instantiate(BunkerFog);
            T_BunkerFog.name = BunkerFog.name;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!On) { return; }

        switch (Stage)
        {
            //Set-Up
            case -1: { 
                    Stage = 0;
                    T_HouseFog = (GameObject)Instantiate(HouseFog);
                    T_HouseFog.name = HouseFog.name;

                    T_BunkerFog = (GameObject)Instantiate(BunkerFog);
                    T_BunkerFog.name = BunkerFog.name;
                break;
            }
            //In Barn
            case 0: {
                    if (BarnArea.GetComponent<DidPlayerCollide>().IsTriggered) {
                        BarnArea.SetActive(false);
                        Stage = 1;
                    }
                    break;
            }
            //Gnomes Attack Barn
            case 1: {

                    if (OldLevel == SpawnManager.OldLevel && SpawnManager.OldLevel == SpawnManager.CurrentLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = false;
                        //Activate all spawners
                        SpawnManager.ActivateAllSpawnersInCurrentRound();
                        Debug.Log("Round Start!");
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        SpawnManager.ToggleAll = true;
                        Debug.Log("Round Over!");
                        Stage = 2;
                        T_HouseFog.SetActive(false);
                    }
                    break;
            }
            //Unlock House
            case 2: {
                    break;
            }
            //Gnomes Attack House
            case 3: {
                    if (OldLevel == SpawnManager.OldLevel && SpawnManager.OldLevel == SpawnManager.CurrentLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = false;
                        //Activate all spawners
                        SpawnManager.ActivateAllSpawnersInCurrentRound();
                        Debug.Log("Round Start!");
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        SpawnManager.ToggleAll = true;
                        Debug.Log("Round Over!");
                        Stage = 4;
                        T_BunkerFog.SetActive(false);
                    }
                    break;
            }
            //Unlock Bunker
            case 4: {
                    break;
            }
            //Gnomes Attack Bunker
            case 5: {
                    if (OldLevel == SpawnManager.OldLevel && SpawnManager.OldLevel == SpawnManager.CurrentLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = false;
                        //Activate all spawners
                        SpawnManager.ActivateAllSpawnersInCurrentRound();
                        Debug.Log("Round Start!");
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        SpawnManager.ToggleAll = true;
                        Debug.Log("Round Over!");
                        Stage = 6;
                    }
                    break;
            }
            default: { break; }
        }


    }
}
