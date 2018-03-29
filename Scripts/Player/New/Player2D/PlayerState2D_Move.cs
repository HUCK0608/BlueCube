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
        // 시점변환중이거나 탐지시점이면 속도를 멈추고 return
        if(GameLibrary.Bool_IsCO)
        {
            m_subController.MoveStopX();
            return;
        }

        // 이동방향 가져오기
        Vector2 moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotate(moveDirection);

        // 중력적용
        m_subController.ApplyGravity();

        ChangeIdleState();
        ChangeAttackState();
        ChangeJumpUpState();
    }

    // Idle 상태로 변경
    private void ChangeIdleState()
    {
        // 이동입력이 없다면 Idle 상태로 변경
        if (m_subController.GetMoveDirection().Equals(Vector2.zero))
            m_mainController.ChangeState2D(E_PlayerState2D.Idle);
    }

    // Attack 상태로 변경
    private void ChangeAttackState()
    {
        // 공격키를 눌렀을 경우 무기를 사용할 수 있다면 Attack 상태로 변경
        if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
            if(m_playerManager.PlayerWeapon.CanUse)
                m_mainController.ChangeState2D(E_PlayerState2D.Attack);
    }

    // JumpUp 상태로 변경
    private void ChangeJumpUpState()
    {
        // 점프키를 눌렀을 때 땅에 있으면 JumpUp 상태로 변경
        if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState2D(E_PlayerState2D.JumpUp);

    }
    public override void EndState()
    {
        base.EndState();

        // x속도를 멈춤
        m_subController.MoveStopX();
    }
}
