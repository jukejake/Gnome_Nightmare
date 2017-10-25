namespace OdinTables
{
    //Used to store all the Tables I make using Odin

    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;

    public class OdinTables : SerializedMonoBehaviour {
        public static OdinTables instance;
        void Awake() { instance = this; }
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
        [TableList]
        public List<OnlineItemTable> Ammo = new List<OnlineItemTable>();
        [TabGroup("Parts", false, 1)]
        [TableList]
        public List<OnlineItemTable> Parts = new List<OnlineItemTable>();
        [TabGroup("Misc", false, 2)]
        [TableList]
        public List<OnlineItemTable> Misc = new List<OnlineItemTable>();
    }
    public class OnlineItemTable {
        [LabelWidth(50)]
        public GameObject Item;
        [HorizontalGroup("Summary"), LabelWidth(60)]
        public string Summary;
        [HorizontalGroup("Stats"), LabelWidth(50)]
        [MinValue(0)]
        public float Price = 0;
        [HorizontalGroup("Stats"), LabelWidth(50)]
        [MinValue(0)]
        public int Amount = 1;
    }
}