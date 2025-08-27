using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    private DoorAnimation doorAnimation;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] Item.KeyType keyType;

    private bool isDoorOpen = false;

    private void Awake()
    {
        doorAnimation = GetComponent<DoorAnimation>();
    }

    public void PlayDoorOpenSound()
    {
        AudioManager.PlaySound(TypeOfSoundEffect.DoorOpen);
    }

    public void PlayDoorCloseSound()
    {
        AudioManager.PlaySound(TypeOfSoundEffect.DoorClose);
    }

    private void HandleDoorInteraction()
    {
        if (isDoorOpen) return;

        Item selectedItem = inventoryManager.GetSelectedItem(false);
        if (selectedItem != null &&
            selectedItem.itemType == Item.ItemType.Key &&
            selectedItem.keyType == doorAnimationKeyType())
        {
            doorAnimation.OpenDoor();
            isDoorOpen = true;
            StartCoroutine(CloseDoorAfterDelay(3f));
        }
    }

    private Item.KeyType doorAnimationKeyType()
    {
        return keyType;
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        doorAnimation.CloseDoor();
        isDoorOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandleDoorInteraction();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandleDoorInteraction();
        }
    }
}