﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BulletType { PlayerFireBall, EnemyFireBall }

public sealed class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerFireBallPrefab;

    [SerializeField]
    private GameObject m_enemyFireBallPrefab;

    private List<Bullet> m_playerBullets;
    private List<Bullet> m_enemyBullets;

    private int m_playerBulletCount;
    private int m_enemyBulletCount;

    /// <summary>총알 발사(타입, 데미지, 발사위치, 발사방향, 속도, 유지시간)</summary>
    public void ShootBullet(E_BulletType bulletType, int damage, Vector3 position, Vector3 direction, float bulletSpeed, float durationTime)
    {
        Bullet canUseBullet = null;

        // 사용가능한 총알이 있는지 체크
        // 플레이어 총알에서 체크
        if (bulletType.Equals(E_BulletType.PlayerFireBall))
        {
            if (m_playerBulletCount != 0)
            {
                for (int i = 0; i < m_playerBulletCount; i++)
                {
                    if (!m_playerBullets[i].IsUse)
                    {
                        canUseBullet = m_playerBullets[i];
                        break;
                    }
                }
            }
        }
        // 적 총알에서 체크
        else if (bulletType.Equals(E_BulletType.EnemyFireBall))
        {
            if (m_enemyBulletCount != 0)
            {
                for (int i = 0; i < m_enemyBulletCount; i++)
                {
                    if (!m_enemyBullets[i].IsUse)
                    {
                        canUseBullet = m_enemyBullets[i];
                        break;
                    }
                }
            }
        }

        // 사용가능한 총알이 없을 경우 새로운 총알 생성
        if (canUseBullet == null)
            canUseBullet = CreateBullet(bulletType);

        // 총알 발사
        canUseBullet.Shoot(damage, position, direction, bulletSpeed, durationTime);
    }

    // 총알 생성 및 리스트에 추가
    private Bullet CreateBullet(E_BulletType bulletType)
    {
        Bullet newBullet = null;

        switch(bulletType)
        {
            case E_BulletType.PlayerFireBall:
                GameObject newObject = Instantiate(m_playerFireBallPrefab, transform);
                newObject.transform.localPosition = Vector3.zero;
                newBullet = newObject.GetComponent<Bullet>();
                m_playerBullets.Add(newBullet);
                m_playerBulletCount++;
                break;
            case E_BulletType.EnemyFireBall:
                newBullet = Instantiate(m_enemyFireBallPrefab, transform).GetComponent<Bullet>();
                m_enemyBullets.Add(newBullet);
                m_enemyBulletCount++;
                break;
        }

        return newBullet;
    }
}
