﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    // 총알 묶음 스크립트
    private BulletBundle m_bundle;
    public BulletBundle Bundle { get { return m_bundle; } }

    // 총알이 사용중인지 여부
    private bool m_isUsed;
    public bool IsUsed { get { return m_isUsed; } }

    private void Awake()
    {
        m_bundle = transform.GetComponentInParent<BulletBundle>();
    }

    // 총알 발사
    public void Shoot(Vector3 start, Vector3 direction)
    {
        m_isUsed = true;

        // 시작위치로 이동
        transform.position = start;

        // 이동 코루틴 시작
        StartCoroutine(Move(direction));
    }

    // 총알 이동 코루틴
    private IEnumerator Move(Vector3 direction)
    {
        // 누적시간
        float accTime = 0;

        // 정면으로 계속 이동
        while(IsUsed)
        {
            transform.Translate(direction * m_bundle.Stat.Speed);

            // 시간 누적
            accTime += Time.fixedDeltaTime;

            // 지속시간이 지났을 경우 발사 정지
            if (accTime >= m_bundle.Stat.DurationTime)
                EndShoot();

            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    // 총알발사를 멈춤
    public void EndShoot()
    {
        m_isUsed = false;
    }
}
