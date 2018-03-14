﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerBullet3D : MonoBehaviour
{
    // 부모
    private Bullet m_bullet;

    // 태그검사용 string
    private static string m_enemyS = "Enemy";
    private static string m_playerS = "Player";
    private static string m_playerAttackS = "PlayerAttack";
    private static string m_playerSkillS = "PlayerSkill";

    private void Awake()
    {
        m_bullet = GetComponentInParent<Bullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적일 경우에만 데미지를 입힘
        if(other.tag == m_enemyS)
        {
            EnemyStat enemyStat = other.GetComponentInParent<EnemyStat>();

            enemyStat.Hit(m_bullet.Bundle.Stat.Damage);

            m_bullet.EndShoot();
        }
        // 플레이어가 아니고 적이 아닐경우
        else if(other.tag != m_playerS && other.tag != m_playerAttackS && other.tag != m_playerSkillS && other.tag != GameLibrary.String_EnemyAttack)
        {
            m_bullet.EndShoot();
        }
    }
}
