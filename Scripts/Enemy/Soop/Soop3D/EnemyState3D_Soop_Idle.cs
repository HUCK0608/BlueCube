using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState3D_Soop_Idle : EnemyState3D_Soop
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    private void ChangeStates()
    {
        // 탐지범위에 플레이어가 들어왔을 경우 ShootInit 상태로 변경
        if (m_mainController.Stat.DetectionArea.IsDectected())
            m_mainController.ChangeState3D(E_SoopState3D.ShootInit);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
