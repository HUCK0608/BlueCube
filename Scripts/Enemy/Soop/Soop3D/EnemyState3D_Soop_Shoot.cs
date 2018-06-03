using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState3D_Soop_Shoot : EnemyState3D_Soop
{
    /// <summary>Shoot 모션이 끝났을 경우 true를 반환</summary>
    private bool m_isEndShootMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndShootMotion = false;

        // 폭탄 발사
        EnemyProjectileManager.Instance.UseProjectile(E_EnemyProjectile.Bomb, m_mainController.Stat.ShootPosition3D.position, PlayerManager.Instance.Player3D_Object.transform.position);
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    private void ChangeStates()
    {
        // 모션이 끝났을 경우 ShootInit 상태로 변경
        if (m_isEndShootMotion)
            m_mainController.ChangeState3D(E_SoopState3D.ShootInit);
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 사용)</summary>
    public void CompleteShootMotion()
    {
        m_isEndShootMotion = true;
    }
}
