using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState2D_Soop_Idle : EnemyState2D_Soop
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsViewChange)
            return;

        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    private void ChangeStates()
    {
        // 체력이 다 달면 Dead 상태로 변경
        if (m_mainController.Stat.Hp <= 0)
            m_mainController.ChangeState2D(E_SoopState.Dead);
        // 탐지범위에 플레이어가 들어왔을 경우 ShootInit 상태로 변경
        if(m_mainController.Stat.DetectionArea.IsDectected())
            m_mainController.ChangeState2D(E_SoopState.ShootInit);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
