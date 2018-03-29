using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D_Move : PlayerState2D
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 이동방향 가져오기
        Vector2 moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotate(moveDirection);

        // 중력적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 공격키를 눌렀을 경우 무기를 사용할 수 있다면 Attack 상태로 변경
        if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
        {
            if (m_playerManager.Weapon.CanUse)
                m_mainController.ChangeState2D(E_PlayerState2D.Attack);
        }
        // 점프키를 눌렀을 때 땅에 있으면 JumpUp 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState2D(E_PlayerState2D.JumpUp);
        }
        // 이동입력이 없거나 플레이어가 멈춰야 하는 상황이면 Idle 상태로 변경
        else if (m_subController.GetMoveDirection().Equals(Vector2.zero) || GameLibrary.Bool_IsPlayerStop)
        {
            m_mainController.ChangeState2D(E_PlayerState2D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();

        // x속도를 멈춤
        m_subController.MoveStopX();
    }
}
