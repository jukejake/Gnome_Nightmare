using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Random_Utils;

namespace EnemySpawners { 
    
    public class Spawner_Hub : SerializedMonoBehaviour {

        [System.NonSerialized]
        public bool Toggle = false;

        [HorizontalGroup("Group 0", 0.5f), LabelWidth(90)]
        public GameObject WorldEnenies;
        [HorizontalGroup("Group 0", 0.5f), LabelWidth(110)]
        public Vector3 SpawnPosRange = new Vector3(0, 0, 0);

        [System.NonSerialized]
        public bool isOff = true;
        [HorizontalGroup("Group 1", 0.33f), LabelWidth(100)]
        public float SpawnInterval = 1.25f;
        [HorizontalGroup("Group 1", 0.33f), LabelWidth(100)]
        public float RoundInterval = 10.0f;
        [HorizontalGroup("Group 1", 0.33f), LabelWidth(70)]
        public float CoolDown = 1.25f;


        private int NumberOfWavesCompleted = 0;


        public List<GameObject> SpawnPositions = new List<GameObject>();
        public List<Enemies> EnemiesToSpawn = new List<Enemies>(1);


        public void Update() {
            if (isOff) { return; }
            CoolDown -= Time.deltaTime;
            if (CoolDown < 0) {
                CoolDown = SpawnInterval;
                SpawnMob();
            }
        }

        void SpawnMob() {
            if (isOff) { return; }
            // Go through all wave components until we find something to spawn
            foreach (Enemies et in EnemiesToSpawn) {
                //Debug.Log("["+et.NumberSpawned+"]/["+(et.StartingAmount + et.IncreasedAmount)+"] B["+et.isOff+"]");
                if (et.NumberSpawned < (et.StartingAmount + et.IncreasedAmount) && et.isOff == false) {
                    et.NumberSpawned++;

                    Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-SpawnPosRange.x, 0.0f, -SpawnPosRange.z), new Vector3(SpawnPosRange.x, SpawnPosRange.y, SpawnPosRange.z));
                    int number = RandomUtils.RandomInt(0, SpawnPositions.Count-1);
                    float speed = RandomUtils.RandomFloat(et.SpeedRange.x, et.SpeedRange.y);

                    GameObject tempObj = Instantiate(et.Enemy, SpawnPositions[number].transform.position + RandomPosition, SpawnPositions[number].transform.rotation);
                    tempObj.transform.SetParent(WorldEnenies.transform);
                    et.IncreaseStats(tempObj);
                    tempObj.name = et.Enemy.name;
                    tempObj.GetComponent<NavMeshAgent>().speed = speed;
                    break;
                }
                else if ((et.NumberSpawned >= (et.StartingAmount + et.IncreasedAmount) && et.isOff == false) || (et.NumberSpawned == 0 && et.isOff == true)) {
                    //Debug.Log("[" + et.NumberSpawned + "] [" + et.StartingAmount + "][" + et.IncreasedAmount + "]" + et.Enemy.name.ToString());
                    et.isOff = true;
                    et.EndOfWaveComp = true;
                    NumberOfWavesCompleted++;
                }
            }
           

            if (NumberOfWavesCompleted >= EnemiesToSpawn.Count) {
                NumberOfWavesCompleted = 0;
                foreach (Enemies et in EnemiesToSpawn) {
                    //Debug.Log("[" + et.NumberSpawned + "] [" + et.StartingAmount + "][" + et.IncreasedAmount + "]" + et.Enemy.name.ToString());
                    et.NumberSpawned = 0;
                    et.EndOfWaveComp = false;
                }
                isOff = true;
                CoolDown = RoundInterval;
                Interface_SpawnTable.instance.CheckAllSpawners();
            }
        }


        public void IncreaseAllStats() {
            foreach (Enemies et in EnemiesToSpawn) { et.IncreaseStats(); }
        }
        public void IncreaseRoundStats(int RoundNumber) {
            foreach (Enemies et in EnemiesToSpawn) {
                if (et.LastAvtiveRound == RoundNumber) { et.IncreaseStats(); }
            }
        }

        public int TotalEnemyCount() {
            int totalEnemyCount = 0;
            foreach (Enemies et in EnemiesToSpawn) {
                totalEnemyCount += (et.StartingAmount + et.IncreasedAmount);
            }
            return totalEnemyCount;
        }

    }
    
    //public interface ISpawnTables { }
    public class Enemies : ISpawnTables{
        public Enemies() { }
        [System.NonSerialized]
        public bool isOff = true;


        [TableColumnWidth(100)]
        [HorizontalGroup("Enemies", 0.6f), LabelWidth(50)]
        public GameObject Enemy;
        [HorizontalGroup("Enemies", 0.4f), LabelWidth(100)]
        public int StartingAmount;

        [HorizontalGroup("Active", 0.5f), LabelWidth(50)]
        public int StartAt = 1;
        [HorizontalGroup("Active", 0.5f), LabelWidth(80)]
        public int ActiveEvery = 1;
        [System.NonSerialized]
        public int LastAvtiveRound = 0;

        [HorizontalGroup("Misc", 0.20f), LabelWidth(80)]
        public bool RandomIncrease = false;
        private bool NotRandomIncrease() { return !this.RandomIncrease; }
        private bool IsInEditMode() { return !Application.isPlaying; }
        
        [HorizontalGroup("Misc", 0.80f), MinMaxSlider(1, 20, true), LabelWidth(90)]
        public Vector2 SpeedRange;

        [System.NonSerialized]
        public int NumberSpawned = 0;
        [System.NonSerialized]
        public bool EndOfWaveComp = false;


        [ShowIf("RandomIncrease")]
        public EnemiesStatsRange IncreaseByRange = new EnemiesStatsRange();
        [ShowIf("NotRandomIncrease")]
        public EnemiesStatsInt IncreaseByInt = new EnemiesStatsInt();

        [System.NonSerialized]
        public  int IncreasedAmount;
        private int IncreasedHealth;
        private int IncreasedArmour;
        private int IncreasedDamage;
        private int IncreasedExp;
        private int IncreasedPoints;
    
        public void IncreaseStats() {
            if (RandomIncrease) {
                IncreasedAmount += RandomUtils.RandomInt(IncreaseByRange.Amount.x, IncreaseByRange.Amount.y);
                IncreasedHealth += RandomUtils.RandomInt(IncreaseByRange.Health.x, IncreaseByRange.Health.y);
                IncreasedArmour += RandomUtils.RandomInt(IncreaseByRange.Armour.x, IncreaseByRange.Armour.y);
                IncreasedDamage += RandomUtils.RandomInt(IncreaseByRange.Damage.x, IncreaseByRange.Damage.y);
                IncreasedExp    += RandomUtils.RandomInt(IncreaseByRange.Exp   .x, IncreaseByRange.Exp   .y);
                IncreasedPoints += RandomUtils.RandomInt(IncreaseByRange.Points.x, IncreaseByRange.Points.y);
            }
            else {
                IncreasedAmount += IncreaseByInt.Amount;
                IncreasedHealth += IncreaseByInt.Health;
                IncreasedArmour += IncreaseByInt.Armour;
                IncreasedDamage += IncreaseByInt.Damage;
                IncreasedExp    += IncreaseByInt.Exp   ;
                IncreasedPoints += IncreaseByInt.Points;
            }
        }
        public void IncreaseStats(GameObject ThisEnemy) {
            ThisEnemy.GetComponent<EnemyStats>().MaxHealth += IncreasedHealth;
            ThisEnemy.GetComponent<EnemyStats>().Armour.baseValue += IncreasedArmour;
            ThisEnemy.GetComponent<EnemyStats>().Damage.baseValue += IncreasedDamage;
            ThisEnemy.GetComponent<EnemyStats>().Experience += IncreasedExp;
            ThisEnemy.GetComponent<EnemyStats>().Points += IncreasedPoints;
        }
    }
    
    public class EnemiesStatsInt : ISpawnTables {
        public EnemiesStatsInt() { }
    
        [HorizontalGroup("Split", 0.5f)]
    
        [TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 1"), LabelWidth(50)]
        public int Amount;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50)]
        public int Health;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50)]
        public int Points;
    
        [TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 2"), LabelWidth(50)]
        public int Armour;
        [VerticalGroup("Split/Stats 2"), LabelWidth(50)]
        public int Damage;
        [VerticalGroup("Split/Stats 2"), LabelWidth(50)]
        public int Exp;
    }
    
    public class EnemiesStatsRange : ISpawnTables {
        public EnemiesStatsRange() { }
    
        [HorizontalGroup("Split", 0.5f)]
    
        [TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 1"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Amount;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Health;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Points;
    
        [TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 2"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Armour;
        [VerticalGroup("Split/Stats 2"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Damage;
        [VerticalGroup("Split/Stats 2"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Exp;
    }
}