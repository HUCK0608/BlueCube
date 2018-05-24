using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PutThrow : PlayerState3D
{
    // 놓기 애니메이션이 종료되었는지 체크하는 변수
    private bool m_isEndPutThrowMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndPutThrowMotion = false;
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 애니메이션과 오브젝트 놓기 과정이 끝나면 Idle 상태로 변경
        if(m_isEndPutThrowMotion && m_playerManager.Hand.CurrentPickPutObject.IsPutEnd)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>오브젝트를 놓음 (애니메이션 이벤트에서 호출)</summary>
    public void PutObject()
    {
        m_playerManager.Hand.CurrentPickPutObject.PutObject();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 호출)</summary>
    public void CompletePutThrowMotion()
    {
        m_isEndPutThrowMotion = true;
    }
}
