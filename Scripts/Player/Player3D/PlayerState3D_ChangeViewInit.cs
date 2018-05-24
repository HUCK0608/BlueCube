﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_ChangeViewInit : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 시점변환 실행
        m_playerManager.Skill.ChangeView();
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 시점변환이 실행되거나 취소했을 경우 Idle 상태로 변경
        if(m_playerManager.IsViewChange || !m_playerManager.IsViewChangeReady)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
