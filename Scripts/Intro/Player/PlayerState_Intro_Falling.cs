using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState_Intro_Falling : PlayerState_Intro
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 이동방향 가져오기
        Vector2 moveDirection = m_controller.GetMoveDirection();

        // 이동 및 회전
        m_controller.JumpMoveAndRotate(moveDirection);

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 땅에 착지할경우 Landing 상태로 변경
        if(m_controller.IsGrounded)
        {
            m_controller.ChangeState(E_PlayerState2D.Landing);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
