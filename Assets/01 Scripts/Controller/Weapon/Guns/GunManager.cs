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

    void changeGun(int newGunIndex)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) changeGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) changeGun(1);
    }
}
