using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;
    void Awake() { instance = this; }
    public GameObject player;

    public bool MenuOpen = false;
    private float MenuTimer = 0.0f;
    private MenuManager menuManager;
    private CraftingManager craftingManager;

    private GameObject InventorySlot;
    private Color color = UnityEngine.Color.white;

    private void Start() {
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        menuManager = MenuManager.instance;
        craftingManager = CraftingManager.instance;
        InventorySlot = menuManager.Menu.transform.GetChild(0).gameObject; //0 is "Item_Inventory"
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (MenuTimer > 0.0f) { MenuTimer -= Time.deltaTime; }
        else if (MenuTimer < 0.0f) { MenuTimer = 0.0f; }

        if (Input.GetButton("Tab") && MenuTimer <= 0.0f) {
            //If player is in a menu exit it
            if (MenuOpen) { ExitMenus(); return; }

            //Enter drop menu
            OpenDropMenu();
        }
    }

    public void KillPlayer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnTriggerStay(Collider other) {
        //If player is selecting a 
        if ((Input.GetButton("E") || Input.GetButton("Tab")) && MenuTimer == 0.0f) {
            //If player interacts with an item
            if (other.tag == "Items" && Input.GetButton("E")) { ItemPickUp(other); }
            //If player interacts with a crafting table
            else if (other.tag == "Crafting_Table" && Input.GetButton("E")) { CraftingMenu(other); }
            //If player interacts with a NPC
            else if (other.tag == "NPC") { }
        }
    }
    private void OnTriggerExit(Collider other) {
        //If player interacts with a crafting table
        if (other.tag == "Crafting_Table") { Crafting_Table.instance.CloseCraftingTable(); }
    }

    private void CraftingMenu(Collider other) {
        MenuTimer = 0.3f;
        //If already in the crafting menu then exit it
        if (other.gameObject.transform.GetChild(0).gameObject.activeSelf) { ExitMenus(); }
        else { Crafting_Table.instance.OpenCraftingTable(); }
    }

    private void ItemPickUp(Collider other) {
        GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
        Drop_Inventory DropI = InventorySlot.GetComponent<Drop_Inventory>();

        if (ItemPickUp.GetComponent<Drag_Inventory>() && ItemPickUp.GetComponent<Drag_Inventory>().typeOfItem == Drag_Inventory.Slot.Ammo) {
            Drop_Inventory DropA = menuManager.Ammo_Slot.GetComponent<Drop_Inventory>();
            //If the player has room in their ammo inventory, pick up the item
            if (DropA.NumberOfSlotsFilled < DropA.NumberOfSlotsTotal) {
                MenuTimer = 0.1f;
                //Spawn item
                GameObject Item = (GameObject)Instantiate(ItemPickUp);
                Item.name = ItemPickUp.name;
                //Set its parent to the ammo inventory
                Item.transform.SetParent(DropA.transform);
                //Sets default scale
                Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Incresses slots filled count
                menuManager.Ammo_Slot.GetComponent<Ammo_Inventory>().CombindStacks();
                DropA.NumberOfSlotsFilled = menuManager.Ammo_Slot.transform.childCount-1; //has a chance that and additional ammo can be added into the inventory
                //Destroy item on ground
                Destroy(other.gameObject);
            }
        }
        //If the player has room in their inventory, pick up the item
        else if (DropI.NumberOfSlotsFilled < DropI.NumberOfSlotsTotal) {
            MenuTimer = 0.1f;
            //Spawn item
            GameObject Item = (GameObject)Instantiate(ItemPickUp);
            Item.name = ItemPickUp.name;
            //Set its parent to the inventory
            Item.transform.SetParent(DropI.transform);
            //Sets default scale
            Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //Incresses slots filled count
            DropI.NumberOfSlotsFilled++;
            //Destroy item on ground
            Destroy(other.gameObject);
        }
    }

    private void OpenDropMenu() {
        Cursor.lockState = CursorLockMode.None;
        MenuOpen = true;
        menuManager.EnableGraphicRaycaster(true);
        color.a = 0.20f;
        menuManager.Menu.transform.GetChild(2).GetComponent<Image>().color = color;//2 is "Drop_To_Floor"
        MenuTimer = 0.3f;
    }
    private void CloseDropMenu() {
        Cursor.lockState = CursorLockMode.Locked;
        MenuOpen = false;
        menuManager.EnableGraphicRaycaster(false);
        color.a = 0.0f;
        menuManager.Menu.transform.GetChild(2).GetComponent<Image>().color = color;//2 is "Drop_To_Floor"
        MenuTimer = 0.3f;
    }


    private void ExitMenus() {
        MenuTimer = 0.3f;
        //If a menu is open, close it
        if (MenuOpen) {
            Crafting_Table.instance.CloseCraftingTable();
            CloseDropMenu();
        }
    }
}