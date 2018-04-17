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

        if (GameLibrary.Bool_IsPlayerStop)
            return;

        // 상태 변경
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 수행 키를 누르고 해당 위치에 내려놓을 수 있을 때 PutInit 상태로 변경
        if((Input.GetKeyDown(m_playerManager.Stat.InteractionKey) || Input.GetKeyDown(m_playerManager.Stat.AcceptKey)) && m_playerManager.Hand.CurrentPickPutObject.IsCanPut)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PutInit);
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
