using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class Spawner_Manager : SerializedMonoBehaviour {
    public static Spawner_Manager instance;
    void Awake() { instance = this; }

    public bool ToggleAll = false;
    public float TimeBetweenRounds = 10.0f;
    private int CurrentLevel = 0;
    private int OldLevel = -1;
    private GameObject EnemyInfoUI;

    public EnemySpawnTable enemySpawnTable = new EnemySpawnTable();

    // Use this for initialization
    void Start () {
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            enemySpawnTable.spawners[i].spawnerDetails.number = i;
        }
        DeActivateAllSpawners();
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        if (enemySpawnTable == null) { return; }
        
        ActivateAllSpawnersInCurrentRound();
        //Finds and updates the UI
        if (GameObject.Find("World").transform.Find("Screen_Menu").transform.Find("Enemy Info")) {
            EnemyInfoUI = GameObject.Find("World").transform.Find("Screen_Menu").transform.Find("Enemy Info").gameObject;
            EnemyInfoUI.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave] [" + CheckAliveEnemyCount() + "/" + CheckTotalEnemyCount() + "-Enemies]");
        }
    }

    private void Update() {
        if (enemySpawnTable != null) {
            UpdateUI();
            for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
                if (!ToggleAll && !enemySpawnTable.spawners[i].Toggle && enemySpawnTable.spawners[i].SpawnerPosition != null) {
                    enemySpawnTable.spawners[i].spawnerDetails.UpdateSpawners();
                }
            }
        }
    }

    //Updates the UI and re-activates spawners at the end of the round
    public void UpdateUI() {
        if (EnemyInfoUI != null) { EnemyInfoUI.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave] [" + CheckAliveEnemyCount() + "/" + CheckTotalEnemyCount() + "-Enemies]"); }
        
        //At the end of a round, spawn a new round
        if (OldLevel == CurrentLevel && CheckAliveEnemyCount() == 0) {
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
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            //If spawner is not active or it does not have the component than return
            if (ToggleAll || enemySpawnTable.spawners[i].Toggle || enemySpawnTable.spawners[i].SpawnerPosition == null) { }
            //enemySpawnTable.spawners[i].LastAvtiveRound will be set as the current round... (only for this)
            else if ((CurrentLevel >= enemySpawnTable.spawners[i].StartAt && CurrentLevel == enemySpawnTable.spawners[i].LastAvtiveRound) || (CurrentLevel == enemySpawnTable.spawners[i].StartAt)) {
                //counts amount of enemies to spawn multiplyed by number of waves in spawner
                EnemyCount += (enemySpawnTable.spawners[i].spawnerDetails.TotalEnemyCount() * enemySpawnTable.spawners[i].spawnerDetails.NumberOfWaves);
            }
        }
        return EnemyCount;
    }

    //Checks if all spawners are DeActive
    public void CheckAllSpawners() {
        int countDeActive = 0;
        //Goes through each spawner and checks if they are de-active
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            if (enemySpawnTable.spawners[i].spawnerDetails.DefeatedSpawner) {
                countDeActive += 1;
            }
        }
        //If all spawners are de-active than Allow it to go to the next level (ie. if(OldLevel == CurrentLevel){ActivateAllSpawners();})
        if (countDeActive == enemySpawnTable.spawners.Count) { OldLevel = CurrentLevel; } //Debug.Log("All are DeActive");
        //If some spawners are still active
        else if (countDeActive < enemySpawnTable.spawners.Count) { }//Debug.Log((spawners.Count - countDeActive) + " are still Active");
        //um... Somehow there are more de-active spawners than there are spawners
        else { Debug.Log("Somehow there are " + (countDeActive - enemySpawnTable.spawners.Count) + " more DeActive spawners than there are spawners."); }
    }

    //Activates all spawners in current round
    public void ActivateAllSpawnersInCurrentRound() {
        CurrentLevel += 1;
        //Goes through all spawners
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            //If spawner is not active or it does not have the component than return
            if (enemySpawnTable.spawners[i].Toggle) { }
            else if ((CurrentLevel >= enemySpawnTable.spawners[i].StartAt && CurrentLevel == (enemySpawnTable.spawners[i].LastAvtiveRound + enemySpawnTable.spawners[i].ActiveEvery)) || (CurrentLevel == enemySpawnTable.spawners[i].StartAt)) {
                enemySpawnTable.spawners[i].LastAvtiveRound = CurrentLevel;
                //If the spawner is deactive then active it
                if (enemySpawnTable.spawners[i].spawnerDetails.DefeatedSpawner) {
                    enemySpawnTable.spawners[i].spawnerDetails.DefeatedSpawner = false;
                    enemySpawnTable.spawners[i].spawnerDetails.Round = TimeBetweenRounds;
                    enemySpawnTable.spawners[i].spawnerDetails.spawnCoolDownRemaining = TimeBetweenRounds;
                    enemySpawnTable.spawners[i].spawnerDetails.IncreaseStats();
                }
            }
        }
    }

    //Sets all the spawners to Deactive
    public void DeActivateAllSpawners() {
        //Goes through all spawners
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            if (enemySpawnTable.spawners[i].spawnerDetails.DefeatedSpawner == false) {
                enemySpawnTable.spawners[i].spawnerDetails.DefeatedSpawner = true;
            }
        }
    }

    //Sets the amount of time between spawns
    public void SetAllIntervalBetweenSpawns(float amount) {
        //Goes through all spawners
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            enemySpawnTable.spawners[i].spawnerDetails.Spawn = amount;
        }
    }
    //Sets the amount of time between rounds
    public void SetAllIntervalBetweenRounds(float amount) {
        //Goes through all spawners
        for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
            enemySpawnTable.spawners[i].spawnerDetails.Round = amount;
        }
    }


    public void SpawnGodDamnIt(SpawnerEnemies Prefab, int number, Vector3 randomPosition) {
        GameObject tempObj = Instantiate(Prefab.enemyPrefab, enemySpawnTable.spawners[number].SpawnerPosition.transform.position + randomPosition, enemySpawnTable.spawners[number].SpawnerPosition.transform.rotation);
        tempObj.transform.SetParent(Spawner_Manager.instance.enemySpawnTable.WorldEnenies.transform);
        Prefab.IncreaseStats(tempObj);
        tempObj.name = Prefab.enemyPrefab.name;
    }
}

public interface IFuckUnity { }

public class EnemySpawnTable : IFuckUnity {
    public static EnemySpawnTable instance;
    void Awake() { instance = this; }
    //Where enemies are set to spawn
    public GameObject WorldEnenies;

    //[TableList]
    //[TabGroup("Spawners", false, 0)]
    public List<Spawners> spawners = new List<Spawners>();
}
public class Spawners : IFuckUnity {
    //[TableColumnWidth(50)]
    [HorizontalGroup("Group 0", 0.1f), LabelWidth(50)]
    public bool Toggle;

    [HorizontalGroup("Group 0", 0.3f), LabelWidth(100)]
    public GameObject SpawnerPosition;

    [HorizontalGroup("Group 0", 0.0f), LabelWidth(50)]
    public int StartAt = 1;
    [HorizontalGroup("Group 0", 0.0f), LabelWidth(80)]
    public int ActiveEvery = 1;
    [HideInInspector]
    //[TableColumnWidth(1)]
    public int LastAvtiveRound = 0;

    public SpawnerDetails spawnerDetails = new SpawnerDetails();

}
public class SpawnerDetails : IFuckUnity {

    [HorizontalGroup("Group 1", 0.5f), LabelWidth(110)]
    public bool DefeatedSpawner = false;
    [HorizontalGroup("Group 1", 0.5f), LabelWidth(120)]
    public int NumberOfWaves = 1;
    private int WaveNumber = 0;
    [FoldoutGroup("Interval Between")]
    [HorizontalGroup("Interval Between/Split"), LabelWidth(50)]
    [MinValue(0), MaxValue(10)]
    public float Spawn = 1.25f;
    [HorizontalGroup("Interval Between/Split"), LabelWidth(50)]
    [MinValue(0), MaxValue(60)]
    public float Round = 5.0f;
    [System.NonSerialized]
    public float spawnCoolDownRemaining = 5.0f;
    [FoldoutGroup("Interval Between"), LabelWidth(100)]
    public Vector3 SpawnPosition = new Vector3(0, 0, 0);

    [System.NonSerialized]
    public int number;

    [TableList]
    public List<SpawnerEnemies> spawnerEnemies = new List<SpawnerEnemies>();

    // Update is called once per frame
    public void UpdateSpawners() {
        if (DefeatedSpawner) { return; }
        spawnCoolDownRemaining -= Time.deltaTime;
        if (spawnCoolDownRemaining < 0) {
            spawnCoolDownRemaining = Spawn;
            SpawnMob();
        }
    }

    void SpawnMob() {
        if (DefeatedSpawner) { return; }

        bool SpawnedAMob = false;
        // Go through all wave components until we find something to spawn
        foreach (SpawnerEnemies wc in spawnerEnemies) {
            if (wc.NumberSpawned < wc.Amount) {
                Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-SpawnPosition.x, 0.0f, -SpawnPosition.z), new Vector3(SpawnPosition.x, SpawnPosition.y, SpawnPosition.z));
                wc.NumberSpawned++;
                //Transform tempTransform = EnemySpawnTable.instance.spawners[EnemySpawnTable.instance.spawnerDetails.IndexOf(this)].SpawnerPosition.transform;
                Spawner_Manager.instance.SpawnGodDamnIt(wc, number, RandomPosition);
                SpawnedAMob = true;
                break;
            }
            else if (wc.NumberSpawned >= wc.Amount) { wc.EndOfWaveComp = true; }
        }
        if (spawnerEnemies[spawnerEnemies.Count-1].EndOfWaveComp) {
            WaveNumber++;
            if (SpawnedAMob == false) {
                if (WaveNumber < NumberOfWaves) {
                    foreach (SpawnerEnemies wc in spawnerEnemies) {
                        wc.NumberSpawned = 0;
                        wc.EndOfWaveComp = false;
                    }
                } else {
                    DefeatedSpawner = true;
                    spawnCoolDownRemaining = Round;
                    Spawner_Manager.instance.CheckAllSpawners();
                    foreach (SpawnerEnemies wc in spawnerEnemies) {
                        //wc.IncreaseStats();
                        wc.NumberSpawned = 0;
                        wc.EndOfWaveComp = false;
                    }
                } 
            }
        }
    }
    public void IncreaseStats() {
        foreach (SpawnerEnemies wc in spawnerEnemies){ wc.IncreaseStats(); }
    }

    public int TotalEnemyCount() {
        int totalEnemyCount = 0;
        foreach (SpawnerEnemies wc in spawnerEnemies) {
            totalEnemyCount += wc.Amount;
        }
        return totalEnemyCount;
    }

}
public class SpawnerEnemies : IFuckUnity {
    [TableColumnWidth(260)]
    [VerticalGroup("Enemies"), LabelWidth(80)]
    public GameObject enemyPrefab;
    [VerticalGroup("Enemies"),LabelWidth(80)]
    public int Amount;

    [TableColumnWidth(90)]
    [VerticalGroup("Increase By"), LabelWidth(40)]
    public int Count;
    [VerticalGroup("Increase By"), LabelWidth(40)]
    public int Health;
    [VerticalGroup("Increase By"), LabelWidth(40)]
    public int Points;

    [TableColumnWidth(100)]
    [VerticalGroup("Increase Stats"), LabelWidth(50)]
    public int Armour;
    [VerticalGroup("Increase Stats"), LabelWidth(50)]
    public int Damage;
    [VerticalGroup("Increase Stats"), LabelWidth(50)]
    public int Exp;

    //[HorizontalGroup("Group 2", 0.8f), LabelWidth(90)]
    [System.NonSerialized]
    public int NumberSpawned = 0;
    [System.NonSerialized]
    public bool EndOfWaveComp = false;

    
    //private int IncreasedCount;
    private int IncreasedHealth;
    private int IncreasedPoints;
    private int IncreasedArmour;
    private int IncreasedDamage;
    private int IncreasedExp;

    public void IncreaseStats(){
        Amount += Count;
        IncreasedHealth += Health;
        IncreasedArmour += Armour;
        IncreasedDamage += Damage;
        IncreasedExp    += Exp;
        IncreasedPoints += Points;
    }
    public void IncreaseStats(GameObject ThisEnemy){
        ThisEnemy.GetComponent<EnemyStats>().MaxHealth += IncreasedHealth;
        ThisEnemy.GetComponent<EnemyStats>().Armour.baseValue += IncreasedArmour;
        ThisEnemy.GetComponent<EnemyStats>().Damage.baseValue += IncreasedDamage;
        ThisEnemy.GetComponent<EnemyStats>().Experience += IncreasedExp;
        ThisEnemy.GetComponent<EnemyStats>().Points += IncreasedPoints;
    }
}