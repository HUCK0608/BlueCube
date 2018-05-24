using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickFalling : PlayerState3D
{
    Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 이동방향 가져오기
        m_moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.JumpMoveAndRotate(m_moveDirection);

        // 상태 변경
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 땅에 닿았을 때 실행
        if (m_mainController.IsGrounded)
        {
            // 이동 입력이 없을 경우 PickIdle 상태로 변경
            if(m_moveDirection.Equals(Vector3.zero))
            {
                m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
            }
            // 이동 입력이 있을 경우 PickMove 상태로 변경
            else
            {
                m_mainController.ChangeState3D(E_PlayerState3D.PickMove);
            }
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
