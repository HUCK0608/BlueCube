using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_PickJumpDown : PlayerState3D
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

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 플레이어가 땅에 닿으면 Idle 상태로 변경
        if (m_mainController.IsGrounded)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
