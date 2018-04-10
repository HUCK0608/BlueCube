using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState2D_Idle : PlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        // 모든 속도 멈춤
        m_subController.MoveStopAll();
    }

    private void Update()
    {
        // x속도 멈춤
        m_subController.MoveStopX();

        if (GameLibrary.Bool_IsPlayerStop)
        {
            // 중력 적용
            m_subController.ApplyGravity();
            return;
        }

        // 중력 적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 땅이 아니라면 Falling 상태로 변경
        if(!m_mainController.IsGrounded)
        {
            m_mainController.ChangeState2D(E_PlayerState2D.Falling);
        }
        // 시점변환 키를 눌렀을 경우 시점변환 실행
        else if(Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey))
        {
            m_playerManager.Skill.ChangeView();
        }
        // 점프키를 눌렀을 때 땅에 있으면 JumpUp 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState2D(E_PlayerState2D.JumpUp);
        }
        // 이동 입력이 있다면 Move 상태로 변경
        else if (!m_subController.GetMoveDirection().Equals(Vector2.zero))
        {
            m_mainController.ChangeState2D(E_PlayerState2D.Move);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
