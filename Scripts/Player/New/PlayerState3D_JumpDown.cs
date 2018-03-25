using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_JumpDown : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 관찰 시점이 아닐경우에만 이동 및 회전이 가능하게 함
        if (!GameManager.Instance.CameraManager.IsObserve)
        {
            Vector3 moveDirection = m_subController.GetMoveDirection();
            Vector3 mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

            // 이동
            m_subController.Move(mouseDirectionToPlayer, moveDirection);
            // 마우스 방향으로의 회전
            m_subController.RotateHeadAndBody(mouseDirectionToPlayer);
        }
        else
        {
            m_subController.MoveStopXZ();
        }

        // 중력적용
        m_subController.ApplyGravity();

        ChangeIdleState();
    }

    // Idle 상태로 변경
    private void ChangeIdleState()
    {
        // 땅일경우 Idle 상태로 변경
        if (m_mainController.IsGrounded)
            m_mainController.ChangeState3D(E_PlayerState.Idle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
