using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crafting_Table : SerializedMonoBehaviour {

    public static Crafting_Table instance;
    void Awake() { instance = this; }
    private CraftingManager craftingManager;
    private MenuManager menuManager;


    private static int YAxis = 10;
    [FoldoutGroup("Combined Table")]
    [TableMatrix(HorizontalTitle = "Combinations", VerticalTitle = "Groups")]
    public GameObject[,] CombinedTable = new GameObject[3, YAxis];

    private static int YAxis2 = 10;
    [FoldoutGroup("Modifier Table")]
    [TableMatrix(HorizontalTitle = "Modifiers", VerticalTitle = "Modifiers")]
    public GameObject[,] ModifierTable = new GameObject[1, YAxis2];


    public bool IsCrafting = false;
    public int OnSlot = 0;
    private float timer = 0.0f;
    private Color color = UnityEngine.Color.white;

    // Use this for initialization
    void Start() {
        Invoke("DelayedStart", 0.1f);
    }

    private void DelayedStart() {
        craftingManager = CraftingManager.instance;
        menuManager = MenuManager.instance;
    }

    // Update is called once per frame
    void Update() {
        if (IsCrafting) {
            if (timer > 0.0f) { timer -= Time.deltaTime; }
            LeftJoystickPlacement();
        }
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

    public void LeftJoystickPlacement() {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //Debug.Log(x + "] [" + y);

        if ((x >= 0.05f || x <= -0.05f) || (y >= 0.05f || y <= -0.05f)) {
            int oldOnSlot = OnSlot;
            if (OnSlot < 0) { OnSlot = 0; }
            else if (x > 00.3f && y < 0.4f && y > -0.4f) { OnSlot = 1; }
            else if (x < -0.3f && y < 0.4f && y > -0.4f) { OnSlot = 2; }
            else if (x < -0.1f && y > 00.3f) { OnSlot = 3; }
            else if (x > 00.1f && y > 00.3f) { OnSlot = 4; }
            else if (x < -0.1f && y < -0.3f) { OnSlot = 5; }
            else if (x > 00.1f && y < -0.3f) { OnSlot = 6; }
            else { return; }
            //Debug.Log(OnSlot);

            color = UnityEngine.Color.white;
            color.a = 0.2f;
                 if (oldOnSlot == 1) { craftingManager.Output_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 2) { craftingManager.Modifier_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 3) { craftingManager.Combined_First_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 4) { craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 5) { craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 6) { craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color; }

            color = UnityEngine.Color.red;
            color.a = 0.2f;
                 if (OnSlot == 1) { craftingManager.Output_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 2) { craftingManager.Modifier_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 3) { craftingManager.Combined_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 4) { craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 5) { craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 6) { craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color; }
        }

        if (Input.GetButton("CY") && timer <= 0.0f) {
            timer = 0.2f;

            bool InventoryHasRoom = false;
            if (menuManager.Inventory_Slot.GetComponent<Drop_Inventory>().NumberOfSlotsFilled < menuManager.Inventory_Slot.GetComponent<Drop_Inventory>().NumberOfSlotsTotal) { InventoryHasRoom = true; }
            int currentInventorySlot = menuManager.CurrentSlot;

            //pull
            //swap
            //push


            if (OnSlot == 1) { //Output_Slot
                if (craftingManager.Output_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Output_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Output_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Output_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Output_Slot.transform);
                }
                else if (craftingManager.Output_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Output_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 2) { //Modifier_Slot
                if (craftingManager.Modifier_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Modifier_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Modifier_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Modifier_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Modifier_Slot.transform);
                }
                else if (craftingManager.Modifier_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Modifier_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 3) { //Combined_First_Slot
                if (craftingManager.Combined_First_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Combined_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Combined_First_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Combined_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_First_Slot.transform);
                }
                else if (craftingManager.Combined_First_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_First_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 4) { //Combined_Second_Slot
                if (craftingManager.Combined_Second_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Combined_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Combined_Second_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Combined_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_Second_Slot.transform);
                }
                else if (craftingManager.Combined_Second_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_Second_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 5) { //Disassemble_First_Slot
                if (craftingManager.Disassemble_First_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Disassemble_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Disassemble_First_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Disassemble_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_First_Slot.transform);
                }
                else if (craftingManager.Disassemble_First_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_First_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 6) { //Disassemble_Second_Slot
                if (craftingManager.Disassemble_Second_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Disassemble_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Disassemble_Second_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Disassemble_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_Second_Slot.transform);
                }
                else if (craftingManager.Disassemble_Second_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_Second_Slot.transform); currentInventorySlot -= 1; }
            }

            menuManager.CurrentSlot = currentInventorySlot;
        }
    }

    public void DpadPlacement() {

        float x = Input.GetAxis("D-pad X");
        float y = Input.GetAxis("D-pad Y");

        if (x != 0.0f || y != 0) {
            int oldOnSlot = OnSlot;
            if (OnSlot < 0) { OnSlot = 0; }
            else if (x > 00.35f && y < 0.1f && y > -0.1f) { OnSlot = 1; }
            else if (x < -0.35f && y < 0.1f && y > -0.1f) { OnSlot = 2; }
            else if (x > -0.3f && x < -0.1f && y < 00.3f && y > 00.1f) { OnSlot = 3; }
            else if (x < 00.3f && x > 00.1f && y < 00.3f && y > 00.1f) { OnSlot = 4; }
            else if (x > -0.3f && x < -0.1f && y > -0.3f && y < -0.1f) { OnSlot = 5; }
            else if (x < 00.3f && x > 00.1f && y > -0.3f && y < -0.1f) { OnSlot = 6; }
            else { return; }
            Debug.Log(OnSlot);

            color = UnityEngine.Color.white;
            color.a = 0.2f;
                 if ( oldOnSlot == 1 ) {craftingManager.Output_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 2 ) {craftingManager.Modifier_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 3 ) {craftingManager.Combined_First_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 4 ) {craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 5 ) {craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 6 ) {craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color;}

            color = UnityEngine.Color.red;
            color.a = 0.2f;
                 if (OnSlot == 1) { craftingManager.Output_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 2) { craftingManager.Modifier_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 3) { craftingManager.Combined_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 4) { craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 5) { craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 6) { craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color; }
        }
    }
}