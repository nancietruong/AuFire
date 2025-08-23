using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("UI")]
    public Image img;
    public Text countText;
    public Color iColor;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;

    public void InitItem(Item newItem)
    {
        item = newItem;
        if (img == null)
        {
            Debug.LogError("InventoryItem: img is not assigned in the Inspector!", this);
        }
        if (newItem == null)
        {
            Debug.LogError("InventoryItem: newItem is null!");
        }
        else if (newItem.itemSprite == null)
        {
            Debug.LogWarning("InventoryItem: newItem.itemSprite is null for item: " + newItem.name, newItem);
        }
        img.sprite = newItem.itemSprite;

        if (newItem.itemType == Item.ItemType.Key)
        {
            img.color = newItem.itemColor;
        }
        SetCount();
    }

    public void SetCount()
    {
        countText.text = count.ToString();
        bool isCountVisible = count > 1;
        countText.gameObject.SetActive(isCountVisible);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.raycastTarget = false; 
        parentAfterDrag = this.transform.parent; 
        this.transform.SetParent(transform.root); // Move the item to the root of the canvas for easier dragging
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        img.raycastTarget = true;
        this.transform.SetParent(parentAfterDrag); // Return the item to its new parent
        this.transform.localPosition = Vector3.zero;
    }
}
