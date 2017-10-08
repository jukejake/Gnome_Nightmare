using UnityEngine;
using UnityEngine.UI;

public class Item_Pick_Up : MonoBehaviour {
    
    public GameObject InventorySlot;
    private bool ItemPickedUp = false;
    private float Timer = 0.0f;
    private float TabTimer = 0.0f;
    private Color color = UnityEngine.Color.white;

    private PlayerManager playerManager;
    private MenuManager menuManager;
    private ItemsManager itemsManager;

    void Start() {
        playerManager = PlayerManager.instance;
        menuManager = MenuManager.instance;
        itemsManager = ItemsManager.instance;
        
        InventorySlot = menuManager.Menu.transform.GetChild(0).gameObject; //0 is "Item_Inventory"
        //GameObject.Find("Menu").GetComponent<GraphicRaycaster>().enabled = false;
        //InventorySlot = GameObject.FindWithTag("Item_Inventory");
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Items" && Input.GetButton("E") && ItemPickedUp == false) {
            GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
            Drop_Inventory DropI = InventorySlot.GetComponent<Drop_Inventory>();
            if (DropI.NumberOfSlotsFilled < DropI.NumberOfSlotsTotal) {
                ItemPickedUp = true;
                Timer = 0.1f;
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
    }
    private void FixedUpdate() {
        if (Timer <= 0.0f && ItemPickedUp == true) { ItemPickedUp = false; }
        else if (Timer > 0.0f) { Timer -= Time.deltaTime; }


        if (Timer <= 0.0f && Input.GetButton("Tab") && playerManager.MenuOpen == false) {
            playerManager.MenuOpen = true;
            menuManager.EnableGraphicRaycaster(true);
            color.a = 0.10f;
            menuManager.Menu.transform.GetChild(5).GetComponent<Image>().color = color;//5 is "Drop_To_Floor"
            //GameObject.Find("Drop_To_Floor").GetComponent<Image>().color = color;
            Timer = 0.2f;
        }
        else if (Timer <= 0.0f && Input.GetButton("Tab") && playerManager.MenuOpen == true) {
            playerManager.MenuOpen = false;
            menuManager.EnableGraphicRaycaster(false);
            color.a = 0.0f;
            menuManager.Menu.transform.GetChild(5).GetComponent<Image>().color = color;//5 is "Drop_To_Floor"
            //GameObject.Find("Drop_To_Floor").GetComponent<Image>().color = color;
            Timer = 0.2f;
        }
    }
}
