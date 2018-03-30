using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState2D_JumpUp : PlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        // 점프
        m_subController.Jump();
    }

    private void Update()
    {
        Vector2 moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.JumpMoveAndRotate(moveDirection);

        // 중력적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 캐릭터가 최대높이까지 뛰었을 경우 JumpDown 상태로 변경
        if (m_subController.Rigidbody.velocity.y <= 0f)
        {
            m_mainController.ChangeState2D(E_PlayerState2D.JumpDown);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
