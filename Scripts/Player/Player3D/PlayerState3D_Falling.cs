using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Falling : PlayerState3D
{
    private Vector3 m_moveDirection;

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

        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 땅에 착지했을 때 상태 변경
        if(m_mainController.IsGrounded)
        {
            // 이동중이 아니고 자동이동이 비활성화 되어있다면 Idle 상태로 변경
            if(m_moveDirection.Equals(Vector3.zero) && !m_subController.IsOnAutoMove)
            {
                m_mainController.ChangeState3D(E_PlayerState3D.Idle);
            }
            // 이동중이라면 Move 상태로 변경
            else
            {
                m_mainController.ChangeState3D(E_PlayerState3D.Move);
            }
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
