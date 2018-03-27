﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private WorldObject m_worldObject;

    private int m_bulletDamage;
    /// <summary>총알 데미지</summary>
    public int BulletDamage { get { return m_bulletDamage; } }

    // 총알이 이동중인지 체크
    private bool m_isMove;

    private bool m_isUse;
    /// <summary>총알을 사용중이면 true를 반환</summary>
    public bool IsUse { get { return m_isUse; } }

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
    }

    /// <summary>direction * moveSpeed로 총알을 발사. durationTime이 지나면 사라짐</summary>
    public void Shoot(int damage, Vector3 position, Vector3 direction, float bulletSpeed, float durationTime)
    {
        m_bulletDamage = damage;

        // 발사 위치로 이동
        transform.position = position + Vector3.up;
        // 발사 방향으로 회전
        transform.rotation = Quaternion.LookRotation(direction);
        // 활성화
        gameObject.SetActive(true);

        StartCoroutine(Move(bulletSpeed, durationTime));
    }

    // 총알 이동
    private IEnumerator Move(float bulletSpeed, float durationTime)
    {
        m_isUse = true;
        m_isMove = true;

        float addTime = 0f;

        while(true)
        {
            // 시간이 멈춰있지 않은 경우에만 실행
            if (!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                // 유지시간
                addTime += Time.deltaTime;

                if (addTime >= durationTime)
                {
                    m_isMove = false;
                    break;
                }

                // 발사 방향으로 이동
                transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
            }

            yield return null;
        }

        // 이펙트 생성
        GameManager.Instance.EffectManager.CreateEffect(Effect_Type.FBExplosion, transform.position);

        // 멀리 보내기
        transform.localPosition = Vector3.zero;

        m_isUse = false;

        // 비활성화
        gameObject.SetActive(false);
    }

    /// <summary>총알의 이동을 멈추고 안보이게 함</summary>
    public void EndShoot()
    {
        m_isMove = false;
    }
}
