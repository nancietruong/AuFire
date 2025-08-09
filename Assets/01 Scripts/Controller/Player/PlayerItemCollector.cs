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
            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                bool isItemAdded = inventoryManager.AddItem(item);

                if (isItemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
