using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletStat))]
public sealed class BulletBundle : MonoBehaviour
{
    // 총알 스텟
    private BulletStat m_stat;
    public BulletStat Stat { get { return m_stat; } }

    // 총알 모음
    private List<Bullet> m_bullets;

    // 총알 개수
    private int m_bulletAmount;

    private void Awake()
    {
        m_stat = GetComponent<BulletStat>();

        InitBullets();
    }

    // 총알 초기화
    private void InitBullets()
    {
        m_bullets = new List<Bullet>();

        m_bulletAmount = transform.childCount;

        for(int i = 0; i < m_bulletAmount; i++)
        {
            m_bullets.Add(transform.GetChild(i).GetComponent<Bullet>());
            m_bullets[i].gameObject.SetActive(false);
        }
    }

    // 총알 발사
    public void ShootBullet(Vector3 start, Vector3 direction)
    {
        for(int i = 0; i < m_bullets.Count; i++)
        {
            // 총알이 사용중이지 않을 경우 총알 발사
            if(!m_bullets[i].IsUsed)
            {
                m_bullets[i].gameObject.SetActive(true);
                m_bullets[i].Shoot(start, direction);
                return;
            }
        }
    }
}
