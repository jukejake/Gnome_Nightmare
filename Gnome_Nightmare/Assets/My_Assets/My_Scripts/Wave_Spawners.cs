using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wave_Spawners : MonoBehaviour {

    //[Range(-100.0f, 100.0f)]
    public float IntervalBetweenSpawns = 1.25f;
    public float IntervalBetweenRounds = 5.0f;
    public float spawnCoolDownRemaining = 5.0f;
    public int NumberOfRounds = 5;
    private int RoundNumber = 0;

    [System.Serializable]
    public class WaveComponent {
        public GameObject enemyPrefab;
        public int NumberOfMobs;
        [System.NonSerialized]
        public int NumberSpawned = 0;
    }

    public WaveComponent[] waveComps;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() {
        spawnCoolDownRemaining -= Time.deltaTime;
        if (spawnCoolDownRemaining < 0) {
            spawnCoolDownRemaining = IntervalBetweenSpawns;
            SpawnMob();
        }
    }

    void SpawnMob() {

        bool SpawnedAMob = false;

        // Go through all wave components until we find something to spawn
        foreach (WaveComponent wc in waveComps) {
            if (wc.NumberSpawned < wc.NumberOfMobs) {
                Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-10.0f, 1.0f, -10.0f), new Vector3(10.0f, 10.0f, 10.0f));
                wc.NumberSpawned++;
                Instantiate(wc.enemyPrefab, this.transform.position + RandomPosition, this.transform.rotation);
                SpawnedAMob = true;
                break;
            }
        }

        if (SpawnedAMob == false) {
            if (RoundNumber < NumberOfRounds) {
                spawnCoolDownRemaining = IntervalBetweenRounds;
                // Wave complete
                if (transform.parent.childCount > 1) {
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                } else {
                    // Last wave
                    foreach (WaveComponent wc in waveComps) {
                        wc.NumberSpawned = 0;
                        RoundNumber++;
                    }
                }
            } else { Destroy(gameObject); }
        }
    }
}
