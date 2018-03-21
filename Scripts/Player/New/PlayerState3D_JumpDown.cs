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
        // 시점변환중이거나 탐지시점이면 return
        if (GameLibrary.Bool_IsCO)
            return;

        Vector3 moveDirection = m_subController.GetMoveDirection();
        Vector3 mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        // 이동
        m_subController.Move(mouseDirectionToPlayer, moveDirection);
        // 마우스 방향으로의 회전
        m_subController.RotateHeadAndBody(mouseDirectionToPlayer);

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
