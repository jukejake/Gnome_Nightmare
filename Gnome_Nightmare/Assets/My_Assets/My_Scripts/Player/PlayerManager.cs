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

    private int OnSlot = 0;

    private void Start() {
        menuManager = MenuManager.instance;
        craftingManager = CraftingManager.instance;

        InventorySlot = menuManager.Menu.transform.GetChild(0).gameObject; //0 is "Item_Inventory"

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (MenuTimer > 0.0f) {
            MenuTimer -= Time.deltaTime;
            Mathf.Clamp(MenuTimer, 0.0f, 30.0f);
        }
        else if (MenuTimer < 0.0f) { MenuTimer = 0.0f; }

        if ((Input.GetButton("E") || Input.GetButton("Tab")) && MenuTimer == 0.0f) {
            if (MenuOpen) { ExitMenus(); }
        }
        if (Input.GetButton("Tab") && MenuTimer == 0.0f) {
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

        if ((Input.GetButton("E") || Input.GetButton("Tab")) && MenuTimer == 0.0f) {
            if (other.tag == "Crafting_Table" && Input.GetButton("E")) { CraftingMenu(other); }
            else if (other.tag == "Items" && Input.GetButton("E")) { ItemPickUp(other); }
            else if (other.tag == "NPC") { }
            else { }
            
        }
    }

    private void CraftingMenu(Collider other) {
        MenuTimer = 0.3f;
        Cursor.lockState = CursorLockMode.None;
        if (other.gameObject.transform.GetChild(0).gameObject.activeSelf) {
            MenuOpen = false;
            menuManager.EnableGraphicRaycaster(false);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            ExitMenus();
        } else {
            MenuOpen = true;
            menuManager.EnableGraphicRaycaster(true);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            if (Input.GetAxis("D-pad Y") >= 00.2f) {
                if (OnSlot == 0) { OnSlot = 1; }
                else if (OnSlot == 1) { }
                else if (OnSlot == 2) { }
                else if (OnSlot == 3) { }
                else if (OnSlot == 4) { }
                else if (OnSlot == 5) { }
                else if (OnSlot == 6) { }
            }
            if (Input.GetAxis("D-pad Y") <= -0.2f) {
                if (OnSlot == 0) { }
                else if (OnSlot == 1) { }
                else if (OnSlot == 2) { }
                else if (OnSlot == 3) { }
                else if (OnSlot == 4) { }
                else if (OnSlot == 5) { }
                else if (OnSlot == 6) { }
            }


            if (Input.GetButton("Fire3")) {
                if (OnSlot == 0) { //inventory
                    if (InventorySlot.transform.GetChild(menuManager.CurrentSlot).GetComponent<Drag_Inventory>().typeOfItem == Drag_Inventory.Slot.Weapon) {
                        if (craftingManager.Output_Slot.transform.childCount != 0) {
                            MenuTimer = 0.2f;
                            craftingManager.Output_Slot.transform.GetChild(0).SetParent(InventorySlot.transform);
                            InventorySlot.transform.GetChild(menuManager.CurrentSlot).SetParent(craftingManager.Output_Slot.transform);
                            Debug.Log("I1");
                        }
                        else {
                            MenuTimer = 0.2f;
                            InventorySlot.transform.GetChild(menuManager.CurrentSlot).SetParent(craftingManager.Output_Slot.transform);
                            Debug.Log("I2");
                        }
                    }
                } else if (OnSlot == 1) { //Output slot

                } else if (OnSlot == 2) { //Modifier slot

                } else if (OnSlot == 3) { //Combined First Slot

                } else if (OnSlot == 4) { //Combined Second Slot

                } else if (OnSlot == 5) { //Disassemble First Slot

                } else if (OnSlot == 6) { //Disassemble Second Slot

                }
                
            }
        }
    }

    private void ItemPickUp(Collider other) {
        GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
        Drop_Inventory DropI = InventorySlot.GetComponent<Drop_Inventory>();
        if (DropI.NumberOfSlotsFilled < DropI.NumberOfSlotsTotal) {
            MenuTimer = 0.1f;
            GameObject Item = (GameObject)Instantiate(ItemPickUp);
            Item.name = ItemPickUp.name;
            Item.transform.SetParent(DropI.transform);
            Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            InventorySlot.GetComponent<Drop_Inventory>().NumberOfSlotsFilled++;

            Destroy(other.GetComponent<Transform>().gameObject);

            Destroy(other.gameObject);
            Destroy(other.GetComponent<GameObject>());
            Destroy(other.GetComponent<GameObject>().gameObject);
            Destroy(other.GetComponent<SphereCollider>());
        }
    }

    private void ExitMenus() {
        MenuTimer = 0.3f;
        if (MenuOpen) {
            MenuOpen = false;
            menuManager.EnableGraphicRaycaster(false);
            craftingManager.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            color.a = 0.0f;
            menuManager.Menu.transform.GetChild(2).GetComponent<Image>().color = color;//2 is "Drop_To_Floor"
        }
        Cursor.lockState = CursorLockMode.Locked;
    }
}