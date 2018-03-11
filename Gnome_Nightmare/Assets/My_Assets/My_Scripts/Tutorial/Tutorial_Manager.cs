using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using EnemySpawners;

public class Tutorial_Manager : SerializedMonoBehaviour {

    public bool On = true;
    public int Stage = -1;
    public Text EventPrompt;

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
    [BoxGroup(group: "Triggers")]
    public GameObject BarnArea;
    [BoxGroup(group: "Triggers")]
    public GameObject HouseArea;
    [BoxGroup(group: "Triggers")]
    public GameObject BunkerArea;

    private Spawner_Hub SP_Hub;



    // Use this for initialization
    void Start () {
        if (SP_Hub == null) { SP_Hub = GameObject.FindObjectOfType(typeof(Spawner_Hub)) as Spawner_Hub; }
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

                    SpawnManager.ToggleAll = true;
                    EventPrompt.text = "Look around the Barn.";

                    break;
            }
            //In Barn
            case 0: {
                    if (BarnArea.GetComponent<DidPlayerCollide>().IsTriggered) {
                        BarnArea.SetActive(false);
                        Stage = 1;
                        if (SP_Hub != null) {
                            SP_Hub.SpawnAtBarn = true;
                            SP_Hub.SpawnAtHouse = false;
                            SP_Hub.SpawnAtBunker = false;
                        }
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
                        EventPrompt.text = "Gnomes are attacking the Barn.";
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = true;
                        EventPrompt.text = "Look through the House.";
                        Stage = 2;
                        T_HouseFog.SetActive(false);
                    }
                    break;
            }
            //Unlock House
            case 2: {
                    if (HouseArea.GetComponent<DidPlayerCollide>().IsTriggered) {
                        HouseArea.SetActive(false);
                        Stage = 3;
                        if (SP_Hub != null) {
                            SP_Hub.SpawnAtBarn = true;
                            SP_Hub.SpawnAtHouse = true;
                            SP_Hub.SpawnAtBunker = false;
                        }
                    }
                    break;
            }
            //Gnomes Attack House
            case 3: {
                    if (OldLevel == SpawnManager.OldLevel && SpawnManager.OldLevel == SpawnManager.CurrentLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = false;
                        //Activate all spawners
                        SpawnManager.ActivateAllSpawnersInCurrentRound();
                        EventPrompt.text = "Gnomes are attacking the House.";
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = true;
                        EventPrompt.text = "Look for the power switch in the Bunker.";
                        Stage = 4;
                        T_BunkerFog.SetActive(false);
                    }
                    break;
            }
            //Unlock Bunker
            case 4: {
                    if (BunkerArea.GetComponent<DidPlayerCollide>().IsTriggered) {
                        BunkerArea.SetActive(false);
                        Stage = 5;
                        if (SP_Hub != null) {
                            SP_Hub.SpawnAtBarn = true;
                            SP_Hub.SpawnAtHouse = true;
                            SP_Hub.SpawnAtBunker = true;
                        }
                    }
                    break;
            }
            //Gnomes Attack Bunker
            case 5: {
                    if (OldLevel == SpawnManager.OldLevel && SpawnManager.OldLevel == SpawnManager.CurrentLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = false;
                        //Activate all spawners
                        SpawnManager.ActivateAllSpawnersInCurrentRound();
                        EventPrompt.text = "Gnomes are attacking the Bunker.";
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = true;
                        EventPrompt.text = "";
                        Stage = 6;
                    }
                    break;
            }
            case 6: {
                    if (OldLevel == SpawnManager.OldLevel && SpawnManager.OldLevel == SpawnManager.CurrentLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = false;
                        //Activate all spawners
                        SpawnManager.ActivateAllSpawnersInCurrentRound();
                        //Debug.Log("Round Start!");
                    }
                    if (OldLevel+1 == SpawnManager.CurrentLevel && OldLevel+1 == SpawnManager.OldLevel && SpawnManager.EverythingDead) {
                        OldLevel = SpawnManager.CurrentLevel;
                        SpawnManager.ToggleAll = true;
                        //Debug.Log("Round Over!");
                    }
                    break;
            }
            default: { break; }
        }


    }
}
