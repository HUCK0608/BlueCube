using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyWeapon_Long_Pistol : EnemyWeapon
{
    protected override void UseWeapon()
    {
        ShootGun();
    }

    private void ShootGun()
    {
        Vector3 shootDirection = transform.forward;

        BulletManager.Instance.ShootBullet(E_BulletType.EnemyFireBall, m_bulletDamage, transform.position, shootDirection, m_bulletSpeed, m_bulletDurationTime);
    }
}
