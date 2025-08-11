using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : GunBase
{
    [SerializeField] Transform firePos;
    private void Update()
    {
        timer -= Time.deltaTime;

        if (player != null && this.transform.IsChildOf(player.transform))
        {
            AimAtMouse();
        }
    }

    public override void Shoot()
    {
        if (timer > 0)
        {
            return;
        }

        BulletBase bulletInstance = ObjectPooling.Instance.GetCOMP<BulletBase>(bulletPrefab);

        bulletInstance.transform.position = firePos.position;

        bulletInstance.Init(bulletSpeed, bulletDamage, bulletLifetime, firePos.right);
        bulletInstance.gameObject.SetActive(true);

        timer = fireCooldown;
    }
}
