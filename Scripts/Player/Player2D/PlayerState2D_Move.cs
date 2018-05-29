using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState2D_Move : PlayerState2D
{
    private Vector2 moveDirection;

    public override void InitState()
    {
        base.InitState();

        // 먼지 이펙트 실행
        m_playerManager.DustEffectParticle.Play();
    }

    private void Update()
    {
        // 이동방향 가져오기
        moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotate(moveDirection);

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 땅이 아니라면 Falling 상태로 변경
        if (!m_mainController.IsGrounded)
        {
            m_mainController.ChangeState2D(E_PlayerState2D.Falling);
        }
        // 시점변환 키를 눌렀을 때 시점변환이 가능하면 Idle 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey) || Input.GetKeyDown(m_playerManager.Stat.CancelKey))
        {
            if (!m_playerManager.IsViewChange && !m_playerManager.IsViewChangeReady)
                m_playerManager.Skill.ChangeView();
        }
        // 점프키를 눌렀을 때 땅에 있으면 JumpUp 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState2D(E_PlayerState2D.JumpUp);
        }
        // 이동입력이 없거나 플레이어가 멈춰야 하는 상황이면 Idle 상태로 변경
        else if (moveDirection.Equals(Vector2.zero) || GameLibrary.Bool_IsPlayerStop)
        {
            m_mainController.ChangeState2D(E_PlayerState2D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();

        // 먼지 이펙트 정지
        m_playerManager.DustEffectParticle.Stop();
    }
}
