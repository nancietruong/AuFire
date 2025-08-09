using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;

    [Header("UI")]
    public Image img;

    public Transform parentAfterDrag;

    public void InitItem(Item newItem)
    {
        item = newItem;
        img.sprite = newItem.itemSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.raycastTarget = false; // Disable raycast target to allow dragging
        parentAfterDrag = this.transform.parent; // Store the current parent
        this.transform.SetParent(transform.root); // Move the item to the root of the canvas for easier dragging
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        img.raycastTarget = true;
        this.transform.SetParent(parentAfterDrag); // Return the item to its original parent
    }
}
