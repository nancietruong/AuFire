using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{

    [SerializeField] protected BulletBase bulletPrefab;

    protected PlayerController player;
    [SerializeField] protected float bulletSpeed, bulletDamage, bulletLifetime, fireCooldown;
    protected float timer = 0;
    public virtual void Init(PlayerController player)
    {
        this.player = player;
        timer = 0;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    protected void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position);
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (this.transform.eulerAngles.z > 90 && this.transform.eulerAngles.z < 270)
        {
            this.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public abstract void Shoot();
}
