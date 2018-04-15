using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickInit : PlayerState3D
{
    // 들기 애니메이션이 끝났는지 체크하는 변수
    private bool m_isPickAniStop;

    public override void InitState()
    {
        base.InitState();

        m_isPickAniStop = false;

        // 모든 속도를 멈춤
        m_subController.MoveStopAll();

        // 오브젝트 들기
        m_playerManager.Hand.CurrentPickPutObject.PickObject();

        m_isPickAniStop = true;
    }

    private void Update()
    {
        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 들기 애니메이션이 끝나고 오브젝트 들기가 완료됬을 경우 PickIdle 상태로 변경
        if (m_isPickAniStop && m_playerManager.Hand.CurrentPickPutObject.IsPick)
            m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
