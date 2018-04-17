using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickJumpUp : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 점프
        m_subController.Jump();
    }

    private void Update()
    {
        // 이동방향 가져오기
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.JumpMoveAndRotate(moveDirection);

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 캐릭터가 최대높이까지 뛰었을 경우 PickJumpDown 상태로 변경
        if(m_subController.Rigidbody.velocity.y <= 0f)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickJumpDown);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
