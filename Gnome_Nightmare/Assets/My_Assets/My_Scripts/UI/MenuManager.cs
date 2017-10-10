using System.Collections;
using System.Collections.Generic;
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
    public GameObject Armour_Equip_Slots; //contains 3 gameobjects

    private bool WeaponEquiped = false;

    // Use this for initialization
    void Start() {
        EnableGraphicRaycaster(false);
        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    void Update() {
        if (Weapon_Slot.transform.childCount != 0 && !WeaponEquiped) { EquipWeapon(); }
        if (Weapon_Slot.transform.childCount == 0 && WeaponEquiped) { UnEquipWeapon(); }
    }

    public void EquipWeapon() {
        if (Weapon_Slot.transform.GetChild(0).name == "placeholder") { return; }
        WeaponEquiped = true;
        weapon = (GameObject)Instantiate(Weapon_Slot.transform.GetChild(0).GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).gameObject);
        weapon.name = Weapon_Slot.transform.GetChild(0).GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).gameObject.name;
        weapon.transform.SetParent(player.transform);
        weapon.transform.localPosition = new Vector3(0.5f,0.0f,0.6f);
        weapon.transform.rotation = player.transform.rotation;
        weapon.transform.localScale = Weapon_Slot.transform.GetChild(0).GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).gameObject.transform.localScale;
    }
    public void UnEquipWeapon() {
        //if (Weapon_Slot.transform.GetChild(0).name == "placeholder") { return; }
        WeaponEquiped = false;
        Destroy(weapon.gameObject);
    }


    public void EnableGraphicRaycaster(bool enable) {
        Menu.GetComponent<GraphicRaycaster>().enabled = enable;
    }
}
