using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushIdle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }
    
    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 취소 키 또는 상호작용 키를 눌렀을 경우 Idle 상태로 변경
        if (Input.GetKeyDown(m_playerManager.Stat.CancelKey) || Input.GetKeyDown(m_playerManager.Stat.InteractionKey))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
        // 수행하는 키를 누르고 해당 위치에 밀 수 있을 경우 PushMove 상태로 변경
        else if (Input.GetKeyDown(m_playerManager.Stat.AcceptKey) && m_playerManager.Hand.CurrentPushItem.IsCanPush)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Pushing);
        }

    }   

    public override void EndState()
    {
        base.EndState();
    }
}
