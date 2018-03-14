using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyWeapon_Long_Shotgun : EnemyWeapon
{
    // 총알모음
    [SerializeField]
    private BulletBundle m_bulletBundle;

    // 총알간 각도
    [SerializeField]
    private float m_angleBetweenBullets;
    
    // 발사 회수
    [SerializeField]
    private int m_shootCount;

    private Quaternion m_startAngleQuaternion;

    private void Awake()
    {
        // 짝수일경우
        if(m_shootCount % 2 == 0)
        {
            m_startAngleQuaternion = Quaternion.Euler(0, (((m_shootCount / 2) - 1) * -m_angleBetweenBullets) - m_angleBetweenBullets * 0.5f, 0);
            Debug.Log((((m_shootCount / 2) - 1) * -m_angleBetweenBullets) - m_angleBetweenBullets * 0.5f);
        }
        // 홀수일경우
        // 개수 / 2 * -각도를 시작각도로 해줌
        else
        {
            m_startAngleQuaternion = Quaternion.Euler(0, m_shootCount / 2 * -m_angleBetweenBullets, 0);
        }
    }


    protected override void UseWeapon()
    {
        ShootShotGun();
    }

    private void ShootShotGun()
    {
        Vector3 shootDirection = m_startAngleQuaternion * transform.forward;

        for(int i = 0; i < m_shootCount; i++)
        {
            // 첫번째 발사가 아닐경우 각도만큼 돌리면서 발사
            if(i != 0)
                shootDirection = Quaternion.Euler(0, m_angleBetweenBullets, 0) * shootDirection;
            shootDirection = shootDirection.normalized;

            m_bulletBundle.ShootBullet(transform.position, shootDirection);
        }
    }
}
