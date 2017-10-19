using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wave_Spawners : MonoBehaviour {

    public bool DefeatedSpawner = false;

    public float IntervalBetweenSpawns = 1.25f;
    public float IntervalBetweenRounds = 5.0f;
    public float spawnCoolDownRemaining = 5.0f;
    public int WaveNumber = 0;
    public int NumberOfWaves = 1;
    public Vector3 BetweenRandomPosition = new Vector3(0,0,0);
    private GameObject WorldEnenies;


    [System.Serializable]
    public class WaveComponent {
        public GameObject enemyPrefab;
        public int NumberOfMobs;
        [System.NonSerialized]
        public int NumberSpawned = 0;
        [System.NonSerialized]
        public bool EndOfWaveComp = false;
    }

    public WaveComponent[] waveComps;

    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.11f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        if (GameObject.Find("World").transform.Find("Enemies")) {
            WorldEnenies = GameObject.Find("World").transform.Find("Enemies").gameObject;
        }
        else {
            WorldEnenies = new GameObject("Enemies");
            WorldEnenies.transform.SetParent(GameObject.Find("World").transform);
        }
    }

    // Update is called once per frame
    void Update() {
        if (DefeatedSpawner) { return; }
        spawnCoolDownRemaining -= Time.deltaTime;
        if (spawnCoolDownRemaining < 0) {
            spawnCoolDownRemaining = IntervalBetweenSpawns;
            SpawnMob();
        }
    }

    void SpawnMob() {
        if (DefeatedSpawner) { return; }

        bool SpawnedAMob = false;
        // Go through all wave components until we find something to spawn
        foreach (WaveComponent wc in waveComps) {
            if (wc.NumberSpawned < wc.NumberOfMobs) {
                Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-BetweenRandomPosition.x, 0.0f, -BetweenRandomPosition.z), new Vector3(BetweenRandomPosition.x, BetweenRandomPosition.y, BetweenRandomPosition.z));
                wc.NumberSpawned++;
                GameObject temp = Instantiate(wc.enemyPrefab, this.transform.position + RandomPosition, this.transform.rotation);
                temp.transform.SetParent(WorldEnenies.transform);
                SpawnedAMob = true;
                break;
            }
            else if (wc.NumberSpawned >= wc.NumberOfMobs) { wc.EndOfWaveComp = true; }
        }
        if (waveComps[waveComps.Length-1].EndOfWaveComp) {
            WaveNumber++;
            if (SpawnedAMob == false) {
                if (WaveNumber < NumberOfWaves) {
                    foreach (WaveComponent wc in waveComps) {
                        wc.NumberSpawned = 0;
                        wc.EndOfWaveComp = false;
                    }
                } else {
                    DefeatedSpawner = true;
                    spawnCoolDownRemaining = IntervalBetweenRounds;
                    Spawner_Manager.instance.CheckAllSpawners();
                    foreach (WaveComponent wc in waveComps) {
                        wc.NumberSpawned = 0;
                        wc.EndOfWaveComp = false;
                    }
                } 
            }
        }
    }


    public int TotalEnemyCount() {
        int totalEnemyCount = 0;
        foreach (WaveComponent wc in waveComps) {
            totalEnemyCount += wc.NumberOfMobs;
        }
        return totalEnemyCount;
    }

    //Amount, MaxHealth, Armour, Damage, Experience
    public void AddStats(string name, int amount) {

        if (name == "Amount") {
            foreach (WaveComponent wc in waveComps) {
                wc.NumberOfMobs += amount;
            }
        }
        else if (name == "MaxHealth") {
            foreach (WaveComponent wc in waveComps) {
                wc.enemyPrefab.GetComponent<EnemyStats>().MaxHealth += amount;
            }
        }
        else if (name == "Armour") {
            foreach (WaveComponent wc in waveComps) {
                wc.enemyPrefab.GetComponent<EnemyStats>().Armour.AddModifier(amount);
            }
        }
        else if (name == "Damage") {
            foreach (WaveComponent wc in waveComps) {
                wc.enemyPrefab.GetComponent<EnemyStats>().Damage.AddModifier(amount);
            }
        }
        else if (name == "Experience") {
            foreach (WaveComponent wc in waveComps) {
                wc.enemyPrefab.GetComponent<EnemyStats>().Experience += amount;
            }
        }
    }
}
