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
            ExitMenus();
            Cursor.lockState = CursorLockMode.None;
            MenuOpen = true;
            menuManager.EnableGraphicRaycaster(true);
            color.a = 0.20f;
            menuManager.Menu.transform.GetChild(2).GetComponent<Image>().color = color;//2 is "Drop_To_Floor"
            MenuTimer = 0.3f;
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
            if (other.tag == "Crafting_Table" && Input.GetButton("E")) { CraftingMenu(other); }
            //If player interacts with a NPC
            else if (other.tag == "NPC") { }
            
        }
    }

    private void CraftingMenu(Collider other) {
        MenuTimer = 0.3f;
        //Lock the cursor
        Cursor.lockState = CursorLockMode.None;
        //If already in the crafting menu then exit it
        if (other.gameObject.transform.GetChild(0).gameObject.activeSelf) {
            ExitMenus();
        } else {
            //Open crafting menu
            MenuOpen = true;
            menuManager.EnableGraphicRaycaster(true);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            other.GetComponent<Crafting_Table>().IsCrafting = true;
        }
    }

    private void ItemPickUp(Collider other) {
        GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
        Drop_Inventory DropI = InventorySlot.GetComponent<Drop_Inventory>();
        //If the player has room in their inventory, pick up the item
        if (DropI.NumberOfSlotsFilled < DropI.NumberOfSlotsTotal) {
            MenuTimer = 0.1f;
            //Spawn item
            GameObject Item = (GameObject)Instantiate(ItemPickUp);
            Item.name = ItemPickUp.name;
            //Set its parent to the inventory
            Item.transform.SetParent(DropI.transform);
            //Sets default scale
            Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //Incresses slots filled count
            InventorySlot.GetComponent<Drop_Inventory>().NumberOfSlotsFilled++;
            //Destroy item on ground
            Destroy(other.gameObject);
        }
    }

    private void ExitMenus() {
        MenuTimer = 0.3f;
        //If a menu is open, close it
        if (MenuOpen) {
            MenuOpen = false;
            menuManager.EnableGraphicRaycaster(false);
            //Deactivate crafting table menu
            craftingManager.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            craftingManager.gameObject.GetComponent<Crafting_Table>().IsCrafting = false;
            //Hide Drop_To_Floor menu
            color.a = 0.0f;
            menuManager.Menu.transform.GetChild(2).GetComponent<Image>().color = color;//2 is "Drop_To_Floor"
        }
        //Lock cursor 
        Cursor.lockState = CursorLockMode.Locked;
    }
}