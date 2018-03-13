using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyAttack_Long_Normal : EnemyAttack
{
    // 총알모음
    [SerializeField]
    private BulletBundle m_bulletBundle;

    public override void Attack()
    {
        ShootGun();
    }

    private void ShootGun()
    {
        Vector3 shootDirection = GameManager.Instance.PlayerManager.Player3D_GO.transform.position - transform.position;
        shootDirection.y = 0f;

        m_bulletBundle.ShootBullet(transform.position, shootDirection.normalized);

        m_isAttack = false;
    }
}
