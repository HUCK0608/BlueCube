using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D_JumpUp : PlayerState2D
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
        m_subController.MoveAndRotate(moveDirection);

        // 중력적용
        m_subController.ApplyGravity();

        ChangeJumpDownState();
    }

    // JumpDown 상태로 변경
    private void ChangeJumpDownState()
    {
        float velocityY = m_subController.Rigidbody.velocity.y;

        // 캐릭터가 최대높이까지 뛰었을 경우 JumpDown 상태로 변경
        if (velocityY <= 0f)
            m_mainController.ChangeState2D(E_PlayerState2D.JumpDown);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
