using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickMove : PlayerState3D
{
    private Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();
        m_subController.DustEffect.Play();
    }

    private void Update()
    {
        // 방향키 입력 방향을 가져옴
        m_moveDirection = m_subController.GetMoveDirection();

        // 이동 및 회전
        m_subController.MoveAndRotate(m_moveDirection);

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 수행 키를 누르고 해당 위치에 내려놓을 수 있을 때 PutInit 상태로 변경
        if ((Input.GetKeyDown(m_playerManager.Stat.InteractionKey) || Input.GetKeyDown(m_playerManager.Stat.AcceptKey)) && m_playerManager.Hand.CurrentPickPutObject.IsCanPut)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PutInit);
        }
        // 점프키를 눌렀을 때 땅에 있으면 PickJumpUp 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.JumpKey))
        {
            if (m_mainController.IsGrounded)
                m_mainController.ChangeState3D(E_PlayerState3D.PickJumpUp);
        }
        // 이동 입력이 없거나 플레이어가 멈춰야 하는 상황이면 PickIdle 상태로 변경
        else if(m_moveDirection.Equals(Vector3.zero) || GameLibrary.Bool_IsPlayerStop)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PickIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
        m_subController.DustEffect.Stop();
    }
}
