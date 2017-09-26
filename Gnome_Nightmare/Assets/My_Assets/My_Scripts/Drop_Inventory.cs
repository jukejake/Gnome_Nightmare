using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //used for drag and drop

public class Drop_Inventory : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public int NumberOfSlotsTotal = 1;
    public int NumberOfSlotsFilled = 0;
    public Drag_Inventory.Slot typeOfItem = Drag_Inventory.Slot.Inventory;


    void FixedUpdate() {
        NumberOfSlotsFilled = this.transform.childCount;
    }

    void IDropHandler.OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        Drag_Inventory DI = eventData.pointerDrag.GetComponent<Drag_Inventory>();
        if (DI != null && NumberOfSlotsFilled <= NumberOfSlotsTotal) {
            if (typeOfItem == DI.typeOfItem || typeOfItem == Drag_Inventory.Slot.Inventory) {
                DI.parentToReturnTo = this.transform;
            }
            if (typeOfItem == Drag_Inventory.Slot.Drop_To_Floor) {
                DI.parentToReturnTo = null;
                GameObject Item = (GameObject)Instantiate(DI.ItemOnDrop);
                Item.transform.SetParent(GameObject.FindWithTag("Items_Spawn_Here").transform);
                Vector3 temp = GameObject.FindWithTag("Player").transform.position;
                Item.transform.position = new Vector3(temp.x, temp.y + 1.0f, temp.z);

                //Drop Item to floor
                Destroy(DI.gameObject);
                Destroy(DI.placeholder.gameObject);
            }
        }
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        Drag_Inventory DI = eventData.pointerDrag.GetComponent<Drag_Inventory>();
        if (DI != null) {
            if (typeOfItem == DI.typeOfItem || typeOfItem == Drag_Inventory.Slot.Inventory) {
                DI.placeholderParent = this.transform;
            }
        }
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        Drag_Inventory DI = eventData.pointerDrag.GetComponent<Drag_Inventory>();
        if (DI != null && DI.placeholderParent == this.transform) {
            if (typeOfItem == DI.typeOfItem || typeOfItem == Drag_Inventory.Slot.Inventory) {
                DI.placeholderParent = DI.parentToReturnTo;
            }
        }
    }
}
