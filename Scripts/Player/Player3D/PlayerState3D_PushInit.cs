using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushInit : PlayerState3D
{
    bool m_isEndPushInitMotion;

    public override void InitState()
    {
        base.InitState();

        LookObject();
        m_isEndPushInitMotion = false;
    }

    /// <summary>오브젝트를 바라보게 설정</summary>
    private void LookObject()
    {
        // 밀기 오브젝트를
        Vector3 directionToObject = m_playerManager.Hand.CurrentPushItem.transform.position - transform.position;
        directionToObject.y = 0f;
        m_subController.LookRotation(directionToObject.normalized);
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 모션이 끝날 경우 PushIdle 상태로 변경
        if(m_isEndPushInitMotion)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PushIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 호출)</summary>
    public void CompletePushInitMotion()
    {
        m_isEndPushInitMotion = true;
    }
}
