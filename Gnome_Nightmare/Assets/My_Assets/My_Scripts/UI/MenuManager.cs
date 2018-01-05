using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {


    public static MenuManager instance;
    void Awake() { instance = this; }
    public GameObject Menu;
    private GameObject player;
    private GameObject weapon;

    [Space]
    public GameObject Inventory_Slot;
    public GameObject Weapon_Slot;
    [Space]
    //Combined
    public GameObject DTF_Slot; //drop to floor
    public GameObject Ammo_Slot; //contains 3 gameobjects

    private bool WeaponEquiped = false;
    private float timer = 0.0f;
    public float timerValue = 0.20f;
    [System.NonSerialized]
    public int CurrentSlot = -1;

    // Use this for initialization
    private void Start() {
        Invoke("DelayedStart", 0.1f);
    }

    private void DelayedStart() {
        EnableGraphicRaycaster(false);
        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    void Update() {

        if (timer > 0.0f) {
            timer -= Time.deltaTime;
            Mathf.Clamp(timer, 0.0f, 30.0f);
        }
        else if (timer <= 0.0f) { ScrollThroughInventory(); }

        if (Weapon_Slot == null) { return; }
        else if (Weapon_Slot.transform.childCount != 0 && !WeaponEquiped) { EquipWeapon(); }
        else if (Weapon_Slot.transform.childCount == 0 && WeaponEquiped) { UnEquipWeapon(); }
    }

    public void EquipWeapon() {
        if (Weapon_Slot.transform.GetChild(0).name == "placeholder") { return; }
        WeaponEquiped = true;
        weapon = (GameObject)Instantiate(Weapon_Slot.transform.GetChild(0).GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).gameObject);
        weapon.name = "weapon";
        if (weapon.GetComponent<Gun_Behaviour>()) {
            weapon.GetComponent<Gun_Behaviour>().enabled = true;
            if (Weapon_Slot.transform.GetChild(0).GetComponent<ItemStats>()) {
                weapon.GetComponent<Gun_Behaviour>().Stats = Weapon_Slot.transform.GetChild(0).GetComponent<ItemStats>().itemStats;
                //weapon.GetComponent<Gun_Behaviour>().GetStats();
            }
        }
        if (weapon.GetComponent<Eyes_Follow_Cursor>()) { weapon.GetComponent<Eyes_Follow_Cursor>().enabled = true; }
        weapon.transform.SetParent(player.transform);
        GameObject temp = GameObject.Find("GUN:pCylinder387");
        temp.GetComponent<MeshRenderer>().enabled = false;
        weapon.transform.SetParent(temp.transform.parent);
        weapon.transform.position = temp.transform.position; // new Vector3(0.5f, 0.0f, 0.6f);
        weapon.transform.localPosition = temp.transform.localPosition;
        weapon.transform.rotation = temp.transform.rotation; // player.transform.rotation;
        weapon.transform.localRotation = temp.transform.localRotation;
        weapon.transform.localScale = Weapon_Slot.transform.GetChild(0).GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).gameObject.transform.localScale;
    }
    public void UnEquipWeapon() {
        //if (Weapon_Slot.transform.GetChild(0).name == "placeholder") { return; }
        WeaponEquiped = false;
        Destroy(weapon.gameObject);
    }
    public void DropItem() {
        if (CurrentSlot == -1) { return; }

        Drag_Inventory DI = (Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<Drag_Inventory>());
        GameObject Item = (GameObject)Instantiate(DI.ItemOnDrop);
        Item.transform.SetParent(GameObject.FindWithTag("Items_Spawn_Here").transform);
        Item.name = DI.ItemOnDrop.name;// + " Not Prefab";
        Vector3 temp = GameObject.FindWithTag("Player").transform.position;
        Item.transform.position = new Vector3(temp.x, temp.y - 0.50f, temp.z);

        if (Item.transform.GetChild(0).GetComponent<Gun_Behaviour>() && DI.gameObject.GetComponent<ItemStats>())
        { Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats = DI.gameObject.GetComponent<ItemStats>().itemStats; }
        
        //Drop Item to floor
        Destroy(DI.gameObject);
        CurrentSlot -= 1;
    }

    public void ScrollThroughInventory() {
        if (Inventory_Slot == null) { return; }
        int InvSpace = Inventory_Slot.GetComponent<Drop_Inventory>().NumberOfSlotsFilled;
        if (InvSpace == 0) { return; }
        // InvSpace/2;
        if (InvSpace < CurrentSlot) { CurrentSlot = (InvSpace - 1); }

        if (Input.GetAxis("D-pad X") >= 00.2f || Input.GetAxis("Mouse ScrollWheel") >= 00.1f) { //right
            if (CurrentSlot == -1) { CurrentSlot = InvSpace/2; }
            if (CurrentSlot >= 0 && CurrentSlot < InvSpace - 1) { CurrentSlot += 1; }
            else if (CurrentSlot == InvSpace-1) { CurrentSlot = 0; }
            timer = timerValue;
            for (int i = 0; i < InvSpace; i++) { Inventory_Slot.transform.GetChild(i).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f); }
            Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    
        if (Input.GetAxis("D-pad X") <= -0.2f || Input.GetAxis("Mouse ScrollWheel") <= -0.1f) { //left
            if (CurrentSlot == -1) { CurrentSlot = InvSpace/2; }
            if (CurrentSlot > 0 && CurrentSlot <= InvSpace) { CurrentSlot -= 1;}
            else if (CurrentSlot == 0) { CurrentSlot = InvSpace-1; }
            timer = timerValue;
            for (int i = 0; i < InvSpace; i++) { Inventory_Slot.transform.GetChild(i).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f); }
            Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        if (Input.GetAxis("D-pad Y") >= 00.2f) {
            if (CurrentSlot == -1) { return; }
            Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            CurrentSlot = -1;
        }

        if (Input.GetAxis("D-pad Y") <= -0.2f) {
            if (CurrentSlot == -1) { return; }
            Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            CurrentSlot = -1;
        }


        if (PlayerManager.instance.MenuOpen) { return; }


        if (Input.GetButton("Fire3") || Input.GetButton("CB")) {
            if (CurrentSlot == -1) { return; }
            if (Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<Drag_Inventory>().typeOfItem == Drag_Inventory.Slot.Weapon) {
                if (Weapon_Slot.transform.childCount != 0) {
                    timer = timerValue;
                    UnEquipWeapon();
                    Weapon_Slot.transform.GetChild(0).SetParent(Inventory_Slot.transform);
                    Inventory_Slot.transform.GetChild(CurrentSlot).SetParent(Weapon_Slot.transform);
                    Weapon_Slot.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    Inventory_Slot.transform.GetChild(Inventory_Slot.transform.childCount-1).transform.SetSiblingIndex(CurrentSlot);
                    //
                    //Inventory_Slot.transform.GetChild(CurrentSlot).GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
                    CurrentSlot -= 1;
                    
                    EquipWeapon();
                    //Debug.Log("Swap");
                }
                else {
                    timer = timerValue;
                    Inventory_Slot.transform.GetChild(CurrentSlot).SetParent(Weapon_Slot.transform);
                    Weapon_Slot.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    EquipWeapon();
                    //Debug.Log("Push");
                    CurrentSlot -= 1;
                }
            }
        }

        if (Input.GetKeyDown("u")){
            DropItem();
        }
    }

    public void EnableGraphicRaycaster(bool enable) {
        Menu.GetComponent<GraphicRaycaster>().enabled = enable;
    }
}
