using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_PickInit : PlayerState3D
{
    // 들기 애니메이션이 끝났는지 체크하는 변수
    private bool m_isPick;

    public override void InitState()
    {
        base.InitState();

        m_isPick = false;

        // 모든 속도를 멈춤
        m_subController.MoveStopAll();

        // 아이템 들기
        m_playerManager.Hand.CurrentPickItem.PickItem();

        m_isPick = true;
    }

    private void Update()
    {
        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 아이템 들어올리기와 현재 애니메이션 들어올리기가 끝나면 PickIdle 상태로 변경
        if (m_isPick && m_playerManager.Hand.CurrentPickItem.IsPick)
            m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
