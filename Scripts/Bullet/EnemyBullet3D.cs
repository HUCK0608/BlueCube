using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyBullet3D : MonoBehaviour
{
    // 부모
    private Bullet m_bullet;

    private void Awake()
    {
        m_bullet = GetComponentInParent<Bullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어일 경우에 데미지를 입힘
        if (other.tag.Equals(GameLibrary.String_Player))
        {
            GameManager.Instance.PlayerManager.Stat.Hit(m_bullet.Bundle.Stat.Damage);

            m_bullet.EndShoot();
        }
        // 적이나 적의공격이나 플레이어 공격이나 플레이어 스킬이 아닐경우 발사 종료
        else if (other.tag != GameLibrary.String_Enemy && other.tag != GameLibrary.String_EnemyAttack && other.tag != GameLibrary.String_PlayerAttack && other.tag != GameLibrary.String_PlayerSkill)
        {
            m_bullet.EndShoot();
        }
    }
}
