﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_PickJumpUp : PlayerState3D
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
        // 캐릭터가 최대높이까지 뛰었을 경우 PickJumpDown 상태로 변경
        if(m_subController.Rigidbody.velocity.y <= 0f)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickJumpDown);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
