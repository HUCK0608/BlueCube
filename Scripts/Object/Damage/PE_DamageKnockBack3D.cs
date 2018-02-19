﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PE_DamageKnockBack3D : MonoBehaviour
{
    PE_DamageKnockBack m_DamageKnockBack;

    private void Awake()
    {
        m_DamageKnockBack = GetComponentInParent<PE_DamageKnockBack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 충돌했을 경우
        if (other.tag.Equals(GameLibrary.String_Player))
        {
            HitAndKnockBackPlayer();
        }
        // 적이 충돌했을 경우
        else if (other.tag.Equals(GameLibrary.String_Enemy))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();

            // 적에게 데미지
            enemy.Stat.Hit(1);
        }
    }

    /// <summary>플레이어 히트 및 넉백</summary>
    private void HitAndKnockBackPlayer()
    {
        // 플레이어 매니저
        PlayerManager playerManager = GameManager.Instance.PlayerManager;

        // 데미지
        playerManager.Stat.Hit(1);

        // 넉백
        // 튕길 방향 구하기
        Vector3 direction = playerManager.Player3D_GO.transform.position - transform.position;
        direction = direction.normalized;

        // x, z 힘 구하기
        Vector3 force = direction * m_DamageKnockBack.KnockBackPower_XZ;
        // y 힘 구하기
        force.y = 1f * m_DamageKnockBack.KnockBackPower_Y;

        playerManager.AddForce(force, ForceMode.Impulse);
    }
}
