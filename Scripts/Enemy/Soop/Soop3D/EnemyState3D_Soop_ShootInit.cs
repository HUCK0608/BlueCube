using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState3D_Soop_ShootInit : EnemyState3D_Soop
{
    /// <summary>ShootInit 모션이 끝났을 경우 true를 반환</summary>
    private bool m_isEndShootInitMotion;

    private float m_addTime;
    private float m_shootDelay;

    public override void InitState()
    {
        base.InitState();

        m_isEndShootInitMotion = false;

        m_addTime = 0f;
        m_shootDelay = Random.Range(m_mainController.Stat.ShootMinDelay, m_mainController.Stat.ShootMaxDelay);
    }

    private void Update()
    {
        // 플레이어로의 방향구하기
        Vector3 directionToPlayer = PlayerManager.Instance.Player3D_Object.transform.position - transform.position;
        directionToPlayer.y = 0f;

        // 회전
        m_subController.RotateToDirection(directionToPlayer.normalized);

        // 장전 모션이 끝난 뒤에 딜레이 계산
        if (m_isEndShootInitMotion)
            m_addTime += Time.deltaTime;

        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    private void ChangeStates()
    {
        // 플레이어가 탐지범위에서 벗어나면 Idle 상태로 변경
        if (!m_mainController.Stat.DetectionArea.IsDectected())
            m_mainController.ChangeState3D(E_SoopState3D.Idle);
        // 딜레이가 끝나면 Shoot 상태로 변경
        else if (m_addTime >= m_shootDelay)
            m_mainController.ChangeState3D(E_SoopState3D.Shoot);
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 사용)</summary>
    public void CompleteShootInitMotion()
    {
        m_isEndShootInitMotion = true;
    }
}
