using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerBulletCollisionCheck2D : MonoBehaviour
{
    // 총알정보 스크립트
    private Bullet m_bullet;

    private void Awake()
    {
        m_bullet = GetComponentInParent<Bullet>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 2D가 아니면 리턴
        if (!PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D))
            return;

        // 적일 경우
        if (other.tag.Equals(GameLibrary.String_Enemy))
        {
            // 적의 스탯 찾기
            EnemyStat stat = other.GetComponentInParent<EnemyStat>();

            // 데미지 입히기
            stat.Hit(m_bullet.BulletDamage);

            // 해당 지점에서 멈춤
            m_bullet.EndShoot();
        }
        // 플레이어 관련 태그가 아니고 적의 공격이 아니면
        else if(!GameLibrary.Bool_IsPlayerTag(other.tag) && !other.tag.Equals(GameLibrary.String_EnemyAttack))
        {
            // 해당 지점에서 멈춤
            m_bullet.EndShoot();
        }
    }
}
