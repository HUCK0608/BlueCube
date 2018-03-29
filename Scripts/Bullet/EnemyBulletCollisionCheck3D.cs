using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCollisionCheck3D : MonoBehaviour
{
    // 총알정보 스크립트
    private Bullet m_bullet;

    private void Awake()
    {
        m_bullet = GetComponentInParent<Bullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 3D가 아니면 리턴
        if (!PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
            return;

        string collisionTag = other.tag;

        // 플레이어일 경우
        if (collisionTag.Equals(GameLibrary.String_Player))
        {
            // 플레이어에게 데미지 입히기
            PlayerManager.Instance.Hit(m_bullet.BulletDamage);

            // 해당 지점에서 멈춤
            m_bullet.EndShoot();
        }
        // 적, 적 총알, 플레이어 총알, 플레이어 스킬이 아닌경우면
        else if(!collisionTag.Equals(GameLibrary.String_Enemy) && 
                !collisionTag.Equals(GameLibrary.String_EnemyAttack) &&
                !collisionTag.Equals(GameLibrary.String_PlayerAttack) &&
                !collisionTag.Equals(GameLibrary.String_PlayerSkill))
        {
            // 해당 지점에서 멈춤
            m_bullet.EndShoot();
        }
    }
}
