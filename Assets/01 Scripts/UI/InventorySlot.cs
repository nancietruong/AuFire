using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public GameObject selectedFrame;

    private void Awake()
    {
        image = GetComponent<Image>();
        UnselectedSlot();
    }

    public void SelectSlot()
    {
        selectedFrame.SetActive(true);
    }

    public void UnselectedSlot()
    {
        selectedFrame.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        // If this slot is empty, just move the item here
        if (this.transform.childCount == 0)
        {
            draggedItem.parentAfterDrag = this.transform;
        }
        else
        {
            // Swap logic
            InventoryItem targetItem = this.transform.GetComponentInChildren<InventoryItem>();
            if (targetItem != null && targetItem != draggedItem)
            {
                Transform originalParent = draggedItem.parentAfterDrag;
                // Swap parents
                draggedItem.parentAfterDrag = this.transform;
                targetItem.parentAfterDrag = originalParent;

                // Move the target item to the original slot
                targetItem.transform.SetParent(originalParent);
                targetItem.transform.localPosition = Vector3.zero;
            }
        }
    }
}
