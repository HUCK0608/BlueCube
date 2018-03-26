using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D_JumpDown : PlayerState2D
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

        ChangeIdleState();
    }

    // Idle 상태로 변경
    private void ChangeIdleState()
    {
        // 땅일경우 Idle 상태로 변경
        if (m_mainController.IsGrounded)
            m_mainController.ChangeState2D(E_PlayerState2D.Idle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
