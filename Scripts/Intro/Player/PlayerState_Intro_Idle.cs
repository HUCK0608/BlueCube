using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState_Intro_Idle : PlayerState_Intro
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // x속도 멈춤
        m_controller.MoveStopX();

        // 상태 변경
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
        // 이동 입력이 있다면 Move 상태로 변경
        else if (!m_controller.GetMoveDirection().Equals(Vector2.zero))
        {
            m_controller.ChangeState(E_PlayerState2D.Move);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
