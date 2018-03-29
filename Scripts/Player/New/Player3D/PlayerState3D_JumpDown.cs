using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_JumpDown : PlayerState3D
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
        // 마우스 방향 가져오기
        Vector3 mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        // 이동
        m_subController.Move(mouseDirectionToPlayer, m_moveDirection);
        // 마우스 방향으로의 회전
        m_subController.RotateHeadAndBody(mouseDirectionToPlayer);

        // 중력적용
        m_subController.ApplyGravity();

        ChangeIdleState();
        ChangeLadderInitState();
    }

    // Idle 상태로 변경
    private void ChangeIdleState()
    {
        // 땅일경우 Idle 상태로 변경
        if (m_mainController.IsGrounded)
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
    }

    // LadderInit 상태로 바뀔지 체크
    private void ChangeLadderInitState()
    {
        // 이동방향에 사다리가 있으면 LadderInit 상태로 변경
        if (m_subController.CheckLadder.IsOnLadder(m_moveDirection))
        {
            // 해당 이동 방향에 있는 곳에 레이를 쏴서 사다리 스크립트를 저장함
            m_subController.CurrentLadder = m_subController.CheckLadder.GetLadder(m_moveDirection);

            // LadderInit 상태로 변경
            m_mainController.ChangeState3D(E_PlayerState3D.LadderInit);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
