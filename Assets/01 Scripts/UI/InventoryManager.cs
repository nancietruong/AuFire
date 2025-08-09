using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public GameObject inventoryItemPrefab;

    public GameObject mainInventory;
    bool isMainInventoryOpen = false;
    public bool AddItem(Item item)
    {
        // Find an empty slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        Debug.LogWarning("Inventory is full, cannot add item: " + item.name);
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot inventorySlot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
        Time.timeScale = isMainInventoryOpen ? 0 : 1;
    }

    private void ToggleInventory()
    {
        isMainInventoryOpen = !isMainInventoryOpen;
        mainInventory.SetActive(isMainInventoryOpen);
    }
}
