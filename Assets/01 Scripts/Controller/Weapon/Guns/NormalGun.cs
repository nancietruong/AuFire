using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : GunBase
{
    [SerializeField] Transform firePos;
    [SerializeField] GameObject muzzlePrefab;

    private GameObject pooledMuzzle;
    private Coroutine muzzleCoroutine;
    private const float muzzleFlashDuration = 0.07f;

    private void OnEnable()
    {
        if (pooledMuzzle != null)
            pooledMuzzle.SetActive(false);

        pooledMuzzle = null;

        if (muzzleCoroutine != null)
        {
            StopCoroutine(muzzleCoroutine);
            muzzleCoroutine = null;
        }
    }

    private void OnDisable()
    {
        if (pooledMuzzle != null)
            pooledMuzzle.SetActive(false);

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
    }

    public override void Shoot()
    {
        if (timer > 0)
        {
            return;
        }

        BulletBase bulletInstance = ObjectPooling.Instance.GetCOMP<BulletBase>(bulletPrefab);

        bulletInstance.transform.position = firePos.position;
        bulletInstance.transform.rotation = firePos.rotation;
        bulletInstance.Init(bulletSpeed, bulletDamage, bulletLifetime, firePos.right);
        bulletInstance.gameObject.SetActive(true);

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

        AudioManager.PlaySound(TypeOfSoundEffect.Rifle);
    }

    private IEnumerator DisableMuzzleAfterDelay()
    {
        yield return new WaitForSeconds(muzzleFlashDuration);

        if (pooledMuzzle != null)
            pooledMuzzle.SetActive(false);

        muzzleCoroutine = null;
    }
}
