using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BulletManager : MonoBehaviour
{
    // 총알묶음 리스트
    private List<BulletBundle> m_bundle;

    // 스크립트 개수
    private int m_bulletsCount;

    private void Awake()
    {
        InitBullets();
    }

    // Bullets 초기화
    private void InitBullets()
    {
        m_bundle = new List<BulletBundle>();

        BulletBundle[] bullets = transform.GetComponentsInChildren<BulletBundle>();

        // 개수 설정
        m_bulletsCount = bullets.Length;

        // 리스트에 추가
        m_bundle.AddRange(bullets);
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
            m_bundle[i].ChangeBullets();
        }
    }
}
