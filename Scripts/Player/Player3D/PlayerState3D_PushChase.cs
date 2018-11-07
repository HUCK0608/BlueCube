using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_PushChase : PlayerState3D
{
    private Vector3 m_hangPosition;

    public override void InitState()
    {
        base.InitState();

        m_subController.MoveStopXZ();

        m_hangPosition = m_playerManager.Hand.CurrentPushItem.GetNearHangPosition(transform.position);
    }

    private void Update()
    {
        m_hangPosition.y = transform.position.y;

        // 고정 지점으로 이동 및 회전을 함
        m_subController.MoveAndRotateTowards(m_hangPosition);

        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 고정지점에 도착했을 경우 PushInit 상태로 변경
        if(transform.position.Equals(m_hangPosition))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PushInit);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
