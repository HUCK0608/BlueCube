using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderInit : PlayerState3D
{
    private static string m_ladderInitAnimationPath = "LadderInit";

    // 모션이 끝났는지 여부
    private bool m_isEndLadderInitMotion;

    private float m_addTime;

    public override void InitState()
    {
        base.InitState();

        m_subController.LookRotation(m_subController.CheckLadder.LatelyLadder.Forward);

        m_isEndLadderInitMotion = false;

        m_addTime = 0f;
    }

    private void Update()
    {
        m_addTime += Time.deltaTime;

        // 1초이상 LadderInit 애니메이션이 나오고 있지 않을 경우 실행
        if (m_addTime >= 1f && !m_subController.Animator.GetCurrentAnimatorStateInfo(0).IsName(m_ladderInitAnimationPath))
        {
            m_subController.Animator.Play(m_ladderInitAnimationPath);
        }

        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 모션이 끝날경우 LadderIdle 상태로 변경
        if(m_isEndLadderInitMotion)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.LadderIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 사용)</summary>
    public void CompleteLadderInitMotion()
    {
        m_isEndLadderInitMotion = true;
    }
}
