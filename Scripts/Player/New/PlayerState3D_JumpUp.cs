using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_JumpUp : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 점프
        m_subController.Jump();
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
            m_subController.MoveStop();
        }

        ChangeJumpDownState();
    }

    // JumpDown상태로 변경
    private void ChangeJumpDownState()
    {
        float velocityY = m_subController.Rigidbody.velocity.y;

        // 캐릭터가 최대높이까지 뛰었을 경우 JumpDown 상태로 변경
        if (velocityY <= 0f)
            m_mainController.ChangeState3D(E_PlayerState.JumpDown);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
