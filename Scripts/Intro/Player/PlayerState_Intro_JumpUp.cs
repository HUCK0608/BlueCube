using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState_Intro_JumpUp : PlayerState_Intro
{
    public override void InitState()
    {
        base.InitState();

        // 점프
        m_controller.Jump();
    }

    private void Update()
    {
        // 이동 방향 가져오기
        Vector2 moveDirection = m_controller.GetMoveDirection();

        // 이동 및 회전
        m_controller.JumpMoveAndRotate(moveDirection);

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 캐릭터가 최대높이까지 뛰었을 경우 Falling 상태로 변경
        if(m_controller.Rigidbody.velocity.y <= 0f)
        {
            m_controller.ChangeState(E_PlayerState2D.Falling);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
