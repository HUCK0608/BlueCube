using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState_Intro_Move : PlayerState_Intro
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
        m_controller.MoveAndRotate(moveDirection);

        ChangeStates();
    }

    private void ChangeStates()
    {
        // 땅이 아니라면 Falling 상태로 변경
        if (!m_controller.IsGrounded)
        {
            m_controller.ChangeState(E_PlayerState2D.Falling);
        }
        // 점프키를 눌렀을 때 땅에 있으면 JumpUp 상태로 변경
        else if (Input.GetKeyDown(m_controller.Stat.JumpKey))
        {
            if (m_controller.IsGrounded)
                m_controller.ChangeState(E_PlayerState2D.JumpUp);
        }
        // 이동 입력이 없으면 Idle 상태로 변경
        else if (m_controller.Equals(Vector2.zero))
        {
            m_controller.ChangeState(E_PlayerState2D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
