using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyWeapon_Long_Pistol : EnemyWeapon
{
    // 총알모음
    [SerializeField]
    private BulletBundle m_bulletBundle;

    protected override void UseWeapon()
    {
        ShootGun();
    }

    private void ShootGun()
    {
        Vector3 shootDirection = transform.forward;

        m_bulletBundle.ShootBullet(transform.position, shootDirection.normalized);
    }
}
