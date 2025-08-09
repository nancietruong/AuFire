using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    [Header("Only for gameplay")]
    public ItemType itemType;
    public ActionType actionType;

    [Header("Only for UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite itemSprite;

    public enum ItemType
    {
        None,
        Weapon,
        Ammo
    }

    public enum ActionType
    {
        None,
        Shoot,
        Reload,
    }    
}
