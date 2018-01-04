using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BulletManager : MonoBehaviour
{
    // 총알관리 스크립트 묶음
    private List<Bullets> m_bullets;

    // 스크립트 개수
    private int m_bulletsCount;

    private void Awake()
    {
        InitBullets();
    }

    private void InitBullets()
    {
        m_bullets = new List<Bullets>();

        Bullets[] bullets = transform.GetComponentsInChildren<Bullets>();

        // 개수 설정
        m_bulletsCount = bullets.Length;

        // 리스트에 추가
        m_bullets.AddRange(bullets);
    }

    private void Start()
    {
        ChangeBullets();
    }

    // 총알 변경
    public void ChangeBullets()
    {
        for(int i = 0; i < m_bulletsCount; i++)
        {
            m_bullets[i].ChangeBullets();
        }
    }
}
