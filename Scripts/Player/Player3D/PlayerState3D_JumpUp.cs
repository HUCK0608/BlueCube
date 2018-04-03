using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_JumpUp : PlayerState3D
{
    private Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();

        // 점프
        m_subController.Jump();
    }

    private void Update()
    {
        // 이동방향 가져오기
        m_moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotation(m_moveDirection, m_playerManager.Stat.MoveSpeed_Jump);

        // 중력적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 이동방향에 사다리가 있으면 LadderInit 상태로 변경
        if (m_subController.CheckLadder.IsOnLadder(m_moveDirection))
        {
            // 해당 이동 방향에 있는 곳에 레이를 쏴서 사다리 스크립트를 저장함
            m_subController.CurrentLadder = m_subController.CheckLadder.GetLadder(m_moveDirection);

            // LadderInit 상태로 변경
            m_mainController.ChangeState3D(E_PlayerState3D.LadderInit);
        }
        // 캐릭터가 최대높이까지 뛰었을 경우 JumpDown 상태로 변경
        else if(m_subController.Rigidbody.velocity.y <= 0f)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.JumpDown);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
