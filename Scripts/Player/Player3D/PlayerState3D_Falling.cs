using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Falling : PlayerState3D
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

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 땅에 착지할경우 Landing 상태로 변경
        if(m_mainController.IsGrounded)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Landing);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
