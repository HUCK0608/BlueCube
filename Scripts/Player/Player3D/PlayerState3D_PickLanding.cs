using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickLanding : PlayerState3D
{
    // 들기 착지 모션이 끝났는지 여부
    private bool m_isEndPickLandingMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndPickLandingMotion = false;
        m_isEndPickLandingMotion = true;
    }

    private void Update()
    {
        // 이동방향 가져오기
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotate(moveDirection);

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 점프 키를 누를 경우 PickJumpUp 상태로 변경
        if(Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickJumpUp);
        }
        // 착지 모션이 끝날 경우 PickIdle 상태로 변경
        else if(m_isEndPickLandingMotion)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
