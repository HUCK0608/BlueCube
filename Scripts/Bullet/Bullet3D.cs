using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bullet3D : MonoBehaviour
{
    // 부모
    private Bullet m_bullet;

    private void Awake()
    {
        m_bullet = GetComponentInParent<Bullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적일 경우에만 데미지를 입힘
        if(other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();

            enemy.Stat.Hit(m_bullet.Bundle.Stat.Damage);

            m_bullet.EndShoot();
        }
        // 플레이어가 아니고 적이 아닐경우
        else if(other.tag != "Player" && other.tag != "PlayerAttack")
        {
            m_bullet.EndShoot();
        }
    }
}
