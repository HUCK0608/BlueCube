using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickJumpDown : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
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
        // 플레이어가 땅에 닿으면 PickIdle 상태로 변경
        if (m_mainController.IsGrounded)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
