using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickEnd : PlayerState3D
{
    private bool m_isPut;

    public override void InitState()
    {
        base.InitState();

        // x, z속도 멈춤
        m_subController.MoveStopXZ();

        m_isPut = false;

        // 아이템 놓기
        m_playerManager.Hand.CurrentPickItem.PutItem();

        m_isPut = true;
    }

    private void Update()
    {
        // 상태 변경
        ChangeStates();
    }

    private void ChangeStates()
    {
        // 애니메이션과 아이템 모두 놓는 동작이 끝나면 Idle 상태로 변경
        if(m_isPut && m_playerManager.Hand.CurrentPickItem.IsPut)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
