using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PE_DamageKnockBack3D : MonoBehaviour
{
    PE_DamageKnockBack m_damageKnockBack;

    private void Awake()
    {
        m_damageKnockBack = GetComponentInParent<PE_DamageKnockBack>();
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
            EnemyStat enemyStat = other.GetComponentInParent<EnemyStat>();

            // 적에게 데미지
            enemyStat.Hit(1);
        }
    }

    /// <summary>플레이어 히트 및 넉백</summary>
    private void HitAndKnockBackPlayer()
    {
        // 플레이어 매니저
        PlayerManager_Old playerManager = GameManager.Instance.PlayerManager;

        // 이미 충돌된 물체인지 체크
        bool isHit = m_damageKnockBack.IsHit(playerManager.gameObject);

        // 충돌된 물체일경우 리턴
        if (isHit)
            return;

        // 데미지
        playerManager.Stat.Hit(1);

        // 넉백
        // 튕길 방향 구하기
        Vector3 direction = playerManager.Player3D_GO.transform.position - transform.position;
        direction = direction.normalized;

        // x, z 힘 구하기
        Vector3 force = direction * m_damageKnockBack.KnockBackPower_XZ;
        // y 힘 구하기
        force.y = 1f * m_damageKnockBack.KnockBackPower_Y;

        playerManager.AddForce(force, ForceMode.Impulse);
    }
}
