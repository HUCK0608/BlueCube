using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Pushing : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 이동을 완료했을 경우 Idle 상태로 변경
        if (!m_playerManager.Hand.CurrentPushItem.IsMove)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
