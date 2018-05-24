using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PutDefault : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 오브젝트 놓기
        m_playerManager.Hand.CurrentPickPutObject.PutObject();
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 오브젝트 놓기 과정이 끝나면 Idle 상태로 변경
        if(m_playerManager.Hand.CurrentPickPutObject.IsPutEnd)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
