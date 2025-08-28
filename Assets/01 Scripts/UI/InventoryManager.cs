using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<GameObject> inventorySlotGroupList = new List<GameObject>();

    public GameObject inventoryItemPrefab;

    public GameObject mainInventory;
    bool isMainInventoryOpen = false;
    public int maxStackSize = 10;

    public int selectedSlotIndex = -1;

    public event Action<Item> OnSelectedItemChanged;
    private void Start()
    {
        //inventorySlots.Clear();
        foreach (var group in inventorySlotGroupList)
        {
            InventorySlot[] slots = group.GetComponentsInChildren<InventorySlot>(true);
            inventorySlots.AddRange(slots);
        }
        ChangeSelectedSlot(0);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
            Time.timeScale = isMainInventoryOpen ? 0 : 1;
        }
        SelectSlotWithNumber();

    }

    void SelectSlotWithNumber()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int slotNumber);
            if (isNumber && slotNumber > 0 && slotNumber <= inventorySlots.Count)
            {
                int newIndex = slotNumber - 1; // Convert to zero-based index
                if (newIndex != selectedSlotIndex)
                {
                    ChangeSelectedSlot(newIndex);
                }
            } 
        }
    }
    void ChangeSelectedSlot(int newIndex)
    {
        if(selectedSlotIndex >= 0 && selectedSlotIndex < inventorySlots.Count)
        {
            // Unselect the previously selected slot
            inventorySlots[selectedSlotIndex].UnselectedSlot();
        }

        inventorySlots[newIndex].SelectSlot();
        selectedSlotIndex = newIndex;

        Item selectedItem = GetSelectedItem(false);
        OnSelectedItemChanged?.Invoke(selectedItem);
    }
   

    public bool AddItem(Item item)
    {
        // Check if the item is stackable
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackSize)
            {
                if (!item.stackable)
                {
                    continue;
                }
                itemInSlot.count++;
                itemInSlot.SetCount();
                return true;
            }
        }
        // Find an empty slot
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                if (i == selectedSlotIndex)
                {
                    ChangeSelectedSlot(selectedSlotIndex);
                    Debug.Log("Item added to selected slot " + selectedSlotIndex + " :" + item.name);
                }
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


    private void ToggleInventory()
    {
        isMainInventoryOpen = !isMainInventoryOpen;
        mainInventory.SetActive(isMainInventoryOpen);
    }

    public Item GetSelectedItem(bool isUse)
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < inventorySlots.Count)
        {
            InventoryItem itemInSlot = inventorySlots[selectedSlotIndex].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {

                Item item = itemInSlot.item;
                if (isUse)
                {
                    itemInSlot.count--;
                    if (itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.SetCount();
                    }
                }
                return item;
            }
        }
        return null;
    }
}
