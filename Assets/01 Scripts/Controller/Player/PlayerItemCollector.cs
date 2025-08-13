using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    public InventoryManager inventoryManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemToPickUp itemToPickUp = collision.GetComponent<ItemToPickUp>();
            if(itemToPickUp != null)
            {
                bool isItemAdded = inventoryManager.AddItem(itemToPickUp.itemData);

                if (isItemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
