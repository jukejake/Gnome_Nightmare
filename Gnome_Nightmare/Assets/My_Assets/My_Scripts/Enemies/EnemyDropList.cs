using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random_Utils;


public class EnemyDropList : SerializedMonoBehaviour {

    public List<OdinTables.EnemyDropTable> Enemies = new List<OdinTables.EnemyDropTable>();

    public List<int> SpawnRandormDrop(int FromEnemy) {
        List<int> IteamsToSpawn = new List<int>();
        for (int i = 0; i < Enemies[FromEnemy].Items.Count; i++) {
            float Range = Enemies[FromEnemy].Items[i].DropChance;

            if (Range == 100.0f) { IteamsToSpawn.Add(i); }
            else if (Range == 0.0f) { }
            else {
                float RandNum1 = RandomUtils.RandomFloat(0.0f, 100.0f);
                Vector2 randRange = new Vector2(RandNum1-(Range*0.5f), RandNum1+(Range*0.5f));
                float RandNum2 = RandomUtils.RandomFloat(0.0f, 100.0f);

                if ((RandNum2 > randRange.x && RandNum2 < randRange.y) || (RandNum2-100.0f > randRange.x && RandNum2-100.0f < randRange.y) || (RandNum2+100.0f > randRange.x && RandNum2+100.0f < randRange.y)) {
                    IteamsToSpawn.Add(i);
                    //Debug.Log("[" + randRange.x + "/" + randRange.y + "] [" + RandNum2 + "]");
                }
            }
        }
        return IteamsToSpawn;
    }
    public void SpawnItem(int FromEnemy, List<int> SpawnItem, Transform AtPosition) {
        for (int j = 0; j < SpawnItem.Count; j++) {
            for (int i = 0; i < Enemies[FromEnemy].Items.Count; i++) {
                if (i == SpawnItem[j]) {
                    //for (int k = 0; k < Enemies[FromEnemy].Items[i].Amount; k++) {
                    //    GameObject TempItem = Instantiate<GameObject>(Enemies[FromEnemy].Items[i].Item);
                    //    TempItem.name = Enemies[FromEnemy].Items[i].Item.name;
                    //    TempItem.transform.SetParent(GameObject.Find("World").transform.Find("Items").transform);
                    //    TempItem.transform.localPosition = AtPosition.localPosition;
                    //}

                    for (int k = 0; k < Enemies[FromEnemy].Items[i].Amount; k++) {
                        GameObject TempItem = (GameObject)Instantiate(Enemies[FromEnemy].Items[i].Item);
                        GameObject Item = (GameObject)Instantiate(TempItem.GetComponent<Drag_Inventory>().ItemOnDrop);

                        Item.name = TempItem.GetComponent<Drag_Inventory>().ItemOnDrop.name;
                        Item.transform.SetParent(GameObject.Find("World").transform.Find("Items").transform);
                        Item.transform.localPosition = AtPosition.localPosition;

                        if (TempItem.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<ItemStats>().itemStats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats = ToStats;
                        }
                        else if (TempItem.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<ItemStats>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<ItemStats>().itemStats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats = ToStats;
                        }
                        else if (TempItem.GetComponent<Gun_Behaviour>() && Item.transform.GetChild(0).GetComponent<ItemStats>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<Gun_Behaviour>().Stats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats = ToStats;
                        }
                        else if (TempItem.GetComponent<Gun_Behaviour>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<Gun_Behaviour>().Stats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats = ToStats;
                        }
                        Destroy(TempItem); 
                    }


                }
            }
        }
    }
}
