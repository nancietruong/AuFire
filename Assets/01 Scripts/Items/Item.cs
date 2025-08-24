using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    [Header("Only for gameplay")]
    public ItemType itemType;
    public bool isConsumable;

    [Header("Key Settings (only if itemType == Key)")]
    public KeyType keyType;

    [Header("Only for UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite itemSprite;
    public Color itemColor;

    public enum ItemType
    {
        None,
        Weapon,
        Ammo,
        Key
    }

    public enum KeyType
    {
        None,
        Red,
        Blue,
        Green
    }

}
