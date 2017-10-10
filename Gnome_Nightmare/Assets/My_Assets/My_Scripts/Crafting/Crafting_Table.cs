using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;

public class Crafting_Table : SerializedMonoBehaviour {

    public static Crafting_Table instance;
    void Awake() { instance = this; }
    private CraftingManager craftingManager;


    private static int YAxis = 10;
    [FoldoutGroup("Combined Table")]
    [TableMatrix(HorizontalTitle = "Combinations", VerticalTitle = "Groups")]
    public GameObject[,] CombinedTable = new GameObject[3, YAxis];

    private static int YAxis2 = 10;
    [FoldoutGroup("Modifier Table")]
    [TableMatrix(HorizontalTitle = "Modifiers", VerticalTitle = "Modifiers")]
    public GameObject[,] ModifierTable = new GameObject[1, YAxis2];



    // Use this for initialization
    void Start() { craftingManager = CraftingManager.instance; }

    // Update is called once per frame
    void Update() {
    }

    public void CheckModifiers() {
        if (craftingManager.Output_Slot.transform.childCount == 1 && craftingManager.Modifier_Slot.transform.childCount == 1) {
            Debug.Log("Modified");
            for (int i = 0; i < YAxis2; i++) {
                if (ModifierTable[0, i] == null) { return; }

                if (craftingManager.Modifier_Slot.transform.GetChild(0).name == ModifierTable[0, i].name) {
                    craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>().AddAllModifiers(craftingManager.Modifier_Slot.transform.GetChild(0).GetComponent<ItemStats>());
                    craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>().SquishAllModifierValues();


                    Destroy(craftingManager.Modifier_Slot.transform.GetChild(0).gameObject);
                }
            }
        }
    }

    public void CheckCombinations() {
        if (craftingManager.Combined_First_Slot.transform.childCount == 1 && craftingManager.Combined_Second_Slot.transform.childCount == 1 && craftingManager.Output_Slot.transform.childCount == 0) {
            for (int i = 0; i < YAxis; i++) {
                if (CombinedTable[0, i] == null || CombinedTable[1, i] == null || CombinedTable[2, i] == null) { return; }

                if ((craftingManager.Combined_First_Slot.transform.GetChild(0).name == CombinedTable[0, i].name && craftingManager.Combined_Second_Slot.transform.GetChild(0).name == CombinedTable[1, i].name)
                  || (craftingManager.Combined_First_Slot.transform.GetChild(0).name == CombinedTable[1, i].name && craftingManager.Combined_Second_Slot.transform.GetChild(0).name == CombinedTable[0, i].name)) {
                    Debug.Log("Assemble");
                    GameObject Item = (GameObject)Instantiate(CombinedTable[2, i]);
                    Item.name = CombinedTable[2, i].name;
                    Item.transform.SetParent(craftingManager.Output_Slot.transform);
                    Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //Item.GetComponent<ItemStats>().Damage.AddModifier(craftingManager.Combined_First_Slot.transform.GetChild(0).GetComponent<ItemStats>().Damage.GetModifierValue() + craftingManager.Combined_Second_Slot.transform.GetChild(0).GetComponent<ItemStats>().Damage.GetModifierValue());
                    Item.GetComponent<ItemStats>().AddAllModifiers(craftingManager.Combined_First_Slot.transform.GetChild(0).GetComponent<ItemStats>());
                    Item.GetComponent<ItemStats>().AddAllModifiers(craftingManager.Combined_Second_Slot.transform.GetChild(0).GetComponent<ItemStats>());
                    Item.GetComponent<ItemStats>().SquishAllModifierValues();


                    Destroy(craftingManager.Combined_First_Slot.transform.GetChild(0).gameObject);
                    Destroy(craftingManager.Combined_Second_Slot.transform.GetChild(0).gameObject);
                }
            }
        }
    }

    public void CheckDisassemble() {
        if (craftingManager.Output_Slot.transform.childCount == 1 && (craftingManager.Disassemble_First_Slot.transform.childCount == 0 && craftingManager.Disassemble_Second_Slot.transform.childCount == 0)) {
            for (int i = 0; i < YAxis; i++) {
                if (CombinedTable[0, i] == null || CombinedTable[1, i] == null || CombinedTable[2, i] == null) { return; }

                if (craftingManager.Output_Slot.transform.GetChild(0).name == CombinedTable[2, i].name) {
                    Debug.Log("Disassemble");
                    int RandomPercent = Random.Range(1, 9);

                    GameObject Item1 = (GameObject)Instantiate(CombinedTable[0, i]);
                    Item1.name = CombinedTable[0, i].name;
                    Item1.transform.SetParent(craftingManager.Disassemble_First_Slot.transform);
                    Item1.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Item1.GetComponent<ItemStats>().AddAllModifiersByPercent(craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>(), (1.0f - (RandomPercent * 0.1f)));
                    Item1.GetComponent<ItemStats>().SquishAllModifierValues();

                    GameObject Item2 = (GameObject)Instantiate(CombinedTable[1, i]);
                    Item2.name = CombinedTable[1, i].name;
                    Item2.transform.SetParent(craftingManager.Disassemble_Second_Slot.transform);
                    Item2.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Item2.GetComponent<ItemStats>().AddAllModifiersByPercent(craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>(), (RandomPercent * 0.1f));
                    Item2.GetComponent<ItemStats>().SquishAllModifierValues();


                    Destroy(craftingManager.Output_Slot.transform.GetChild(0).gameObject);
                }
            }
        }
    }


}