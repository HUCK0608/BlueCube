using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PutDefault : PlayerState3D
{
    // 놓기 애니메이션이 종료되었는지 체크하는 변수
    private bool m_isPutAniStop;

    public override void InitState()
    {
        base.InitState();

        m_isPutAniStop = false;

        // 오브젝트 놓기
        m_playerManager.Hand.CurrentPickPutObject.PutObject();

        m_isPutAniStop = true;
    }

    private void Update()
    {
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 애니메이션과 오브젝트 놓기 과정이 끝나면 Idle 상태로 변경
        if(m_isPutAniStop && m_playerManager.Hand.CurrentPickPutObject.IsPutEnd)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
