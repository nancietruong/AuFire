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
        if (this.transform.childCount == 0)
        {
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
            item.parentAfterDrag = this.transform; // Set the parent after drag to this slot
        }
    }
}
