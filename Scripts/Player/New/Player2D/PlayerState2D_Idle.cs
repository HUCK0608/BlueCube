using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D_Idle : PlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        // 모든 속도 멈춤
        m_subController.MoveStopAll();
    }

    private void Update()
    {
        // 시점변환중이거나 탐지시점이면 return
        if (GameLibrary.Bool_IsCO)
            return;

        // 중력 적용
        m_subController.ApplyGravity();

        // x 속도 멈추기
        m_subController.MoveStopX();

        ChangeMoveState();
        ChangeAttackState();
        ChangeJumpUpState();
    }

    // Move 상태로 변경
    private void ChangeMoveState()
    {
        // 이동 입력이 있다면 Move 상태로 변경
        if (!m_subController.GetMoveDirection().Equals(Vector2.zero))
            m_mainController.ChangeState2D(E_PlayerState2D.Move);
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
    }
}
