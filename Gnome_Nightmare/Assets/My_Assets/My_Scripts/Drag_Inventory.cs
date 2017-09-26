﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //used for LayoutElement
using UnityEngine;
using UnityEngine.EventSystems; //used for drag and drop

public class Drag_Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public enum Slot { Inventory, Miscellaneous, Weapon, Head, Chest, Legs, Drop_To_Floor };
    public Slot typeOfItem = Slot.Inventory;


    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    public GameObject placeholder = null;
    public GameObject ItemOnDrop = null;

    private Vector2 MouseDifference;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }

        //Create a placeholder
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement placeholder_layoutElement = placeholder.AddComponent<LayoutElement>();
        placeholder_layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        placeholder_layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        placeholder_layoutElement.flexibleWidth = 0;
        placeholder_layoutElement.flexibleHeight = 0;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //Find where the user clicked on the Item
        MouseDifference = eventData.position - new Vector2(transform.position.x, transform.position.y);

        //set the parent(inventory) of the item
        parentToReturnTo = this.transform.parent;
        placeholderParent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        //Allows the OnDrop function to work
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }

        //Item being dragged will follow the pointer
        this.transform.position = eventData.position - MouseDifference;

        if (placeholder.transform.parent != placeholderParent) {
            placeholder.transform.SetParent(placeholderParent);
        }

        //
        int newSiblingIndex = placeholderParent.childCount;
        for (int i = 0; i < placeholderParent.childCount; i++) {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x) {
                if (this.transform.position.x > placeholderParent.GetChild(i).position.x ) {
                    //- (placeholderParent.GetChild(i).GetComponent<RectTransform>().rect.width/2.0f)
                    Debug.Log("BOOM");
                }
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex) {
                    newSiblingIndex--;
                }
               
                break;
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }

        //Return to the correct place
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        //Enable so that you can pick up the Item again
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //DISTROY the placeholder
        Destroy(placeholder);

        //Apply Status boosts from equipment
        //Weapons and stuff
    }
}
