using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BulletBase
{
    public override void Boom(GameObject target)
    {
        this.gameObject.SetActive(false);
        ITakeDamage damageable = target.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Boom(collision.gameObject);
    }
}
