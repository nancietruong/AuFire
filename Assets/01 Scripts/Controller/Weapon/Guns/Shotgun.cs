using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    [SerializeField] Transform firePos;
    [SerializeField] GameObject muzzlePrefab;

    [Header("Shotgun Settings")]
    [SerializeField] int numberOfBullets = 5;
    [SerializeField] float spreadAngle = 15f;

    private GameObject pooledMuzzle;
    private Coroutine muzzleCoroutine;
    private const float muzzleFlashDuration = 0.07f;

    private void OnEnable()
    {
        // Reset pooledMuzzle reference and ensure it's inactive when switching guns
        pooledMuzzle = null;
        if (muzzleCoroutine != null)
        {
            StopCoroutine(muzzleCoroutine);
            muzzleCoroutine = null;
        }
    }
    private void Update()
    {
        timer -= Time.deltaTime;

        if (player != null && this.transform.IsChildOf(player.transform))
        {
            AimAtMouse();
        }

        if (pooledMuzzle != null && pooledMuzzle.activeSelf && !Input.GetMouseButton(0))
        {
            pooledMuzzle.SetActive(false);
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

        // Muzzle flash blink
        if (muzzlePrefab != null && firePos != null)
        {
            if (pooledMuzzle == null)
            {
                pooledMuzzle = ObjectPooling.Instance.GetOBJ(muzzlePrefab);
                pooledMuzzle.transform.SetParent(firePos);
                pooledMuzzle.transform.localPosition = Vector3.zero;
                pooledMuzzle.transform.localRotation = Quaternion.identity;
            }

            if (muzzleCoroutine != null)
            {
                StopCoroutine(muzzleCoroutine);
            }
            pooledMuzzle.SetActive(true);
            muzzleCoroutine = StartCoroutine(DisableMuzzleAfterDelay());
        }

        timer = fireCooldown;
    }
    private IEnumerator DisableMuzzleAfterDelay()
    {
        yield return new WaitForSeconds(muzzleFlashDuration);
        if (pooledMuzzle != null)
            pooledMuzzle.SetActive(false);
    }
}
