using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    [SerializeField] Transform firePos;

    [Header("Shotgun Settings")]
    [SerializeField] int numberOfBullets = 5;
    [SerializeField] float spreadAngle = 15f;

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

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (numberOfBullets - 1);

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector2 direction = rotation * firePos.right;

            BulletBase bulletInstance = ObjectPooling.Instance.GetCOMP<BulletBase>(bulletPrefab);
            bulletInstance.transform.position = firePos.position;
            bulletInstance.Init(bulletSpeed, bulletDamage, bulletLifetime, direction);
            bulletInstance.gameObject.SetActive(true);
        }

        timer = fireCooldown;
    }
}
