using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] List<GunBase> gunList = new List<GunBase>();
    [SerializeField] int currentGunIndex = 0;
    PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();

        for (int i = 0; i < gunList.Count; i++)
        {
            gunList[i].gameObject.SetActive(i  == currentGunIndex);
            gunList[i].Init(player);
        }
    }

    public void ChangeGun(int newGunIndex)
    {
        if (newGunIndex < 0 || gunList.Count == 0 || newGunIndex >= gunList.Count || newGunIndex == currentGunIndex)
        {
            return;
        }

        gunList[currentGunIndex].gameObject.SetActive(false);
        currentGunIndex = newGunIndex;
        gunList[currentGunIndex].gameObject.SetActive(true);
        gunList[currentGunIndex].Init(player);

        if (player != null) player.gun = gunList[currentGunIndex];
    }

    public int GetGunIndexByItem(Item item)
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            if (gunList[i].item == item)
                return i;
        }
        return -1;
    }
}
