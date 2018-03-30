using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickIdle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }
    
    private void Update()
    {
        // x, z 이동을 멈춤
        m_subController.MoveStopXZ();

        if(GameLibrary.Bool_IsPlayerStop)
        {
            // 중력만 적용
            m_subController.ApplyGravity();
            return;
        }

        // 플레이어에서 마우스의 방향을 가져옴
        Vector3 mouseDirectionToPlayer = CameraManager.Instance.GetMouseDirectionToPivot(transform.position);

        // 머리, 몸 회전
        m_subController.RotateHeadAndBody(mouseDirectionToPlayer);

        // 중력적용
        m_subController.ApplyGravity();

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 상호작용키를 눌렀을 때 PickItemEnd 상태로 변경
        if(Input.GetKeyDown(m_playerManager.Stat.InteractionKey) || m_playerManager.IsViewChangeReady)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickEnd);
        }
        // 점프키를 눌렀을 때 땅에 있으면 PickJumpUp 상태로 변경
        else if(Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState3D.PickJumpUp);
        }
        // 이동 입력이 있을경우 PickMove 상태로 변경
        else if(!moveDirection.Equals(Vector3.zero))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickMove);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
