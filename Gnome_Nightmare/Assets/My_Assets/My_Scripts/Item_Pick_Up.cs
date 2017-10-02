using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Pick_Up : MonoBehaviour {

    public bool InInventory;
    public GameObject InventorySlot;
    private bool ItemPickedUp = false;
    private float Timer = 0.0f;
    private float TabTimer = 0.0f;
    private Color color = UnityEngine.Color.white;

    void Start() {
        InInventory = false;
        GameObject.Find("Menu").GetComponent<GraphicRaycaster>().enabled = false;
        InventorySlot = GameObject.FindWithTag("Item_Inventory");
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Items" && Input.GetButton("E") && ItemPickedUp == false) {
            GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
            Drop_Inventory DropI = InventorySlot.GetComponent<Drop_Inventory>();
            if (DropI.NumberOfSlotsFilled < DropI.NumberOfSlotsTotal) {
                ItemPickedUp = true;
                Timer = 0.1f;
                GameObject Item = (GameObject)Instantiate(ItemPickUp);
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


        if (Timer <= 0.0f && Input.GetButton("Tab") && InInventory == false) {
            InInventory = true;
            GameObject.Find("Menu").GetComponent<GraphicRaycaster>().enabled = true;
            color.a = 0.10f;
            GameObject.Find("Drop_To_Floor").GetComponent<Image>().color = color;
            Timer = 0.2f;
        }
        else if (Timer <= 0.0f && Input.GetButton("Tab") && InInventory == true)
        {
            InInventory = false;
            GameObject.Find("Menu").GetComponent<GraphicRaycaster>().enabled = false;
            color.a = 0.0f;
            GameObject.Find("Drop_To_Floor").GetComponent<Image>().color = color;
            Timer = 0.2f;
        }
    }
}
