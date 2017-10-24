namespace OdinTables
{
    //Used to store all the Tables I make using Odin

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class OdinTables : SerializedMonoBehaviour {

    }
    
    public class Table1x5 {
        [TableMatrix(HorizontalTitle = "Row", VerticalTitle = "Col")]
        public GameObject[,] table1x5 = new GameObject[1, 5];
    }
    public class Table2x5 {
        [TableMatrix(HorizontalTitle = "Row", VerticalTitle = "Col")]
        public GameObject[,] table2x5 = new GameObject[2, 5];
    }
    public class Table3x5 {
        [TableMatrix(HorizontalTitle = "Row", VerticalTitle = "Col")]
        public GameObject[,] table3x5 = new GameObject[3, 5];
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

    public class EnemyDropTable {
        [TabGroup("Enemy", false, 0)]
        [LabelWidth (50)]
        public GameObject Enemy;

        [TabGroup("Items", false, 1)]
        public List<ItemDropTable> Items = new List<ItemDropTable>();
    }

    public class ItemDropTable {
        [LabelWidth(50)]
        public GameObject Item;
        [HorizontalGroup("Group 2"), LabelWidth(80)]
        [MinValue(0), MaxValue(100)]
        public float DropChance = 0;
        [HorizontalGroup("Group 2"), LabelWidth(50)]
        public int Amount = 1;
    }

    public class OnlineTable {
        [TabGroup("Ammo", false, 0)]
        public List<OnlineItemTable> Ammo = new List<OnlineItemTable>();
        [TabGroup("Parts", false, 1)]
        public List<OnlineItemTable> Parts = new List<OnlineItemTable>();
        [TabGroup("Misc", false, 2)]
        public List<OnlineItemTable> Misc = new List<OnlineItemTable>();
    }

    public class OnlineItemTable {
        [LabelWidth(50)]
        public GameObject Item;
        [HorizontalGroup("Group 1"), LabelWidth(60)]
        public string Summary;
        [HorizontalGroup("Group 2"), LabelWidth(50)]
        [MinValue(0)]
        public float Price = 0;
        [HorizontalGroup("Group 2"), LabelWidth(50)]
        [MinValue(0)]
        public int Amount = 1;
    }
}