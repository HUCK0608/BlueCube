using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_LadderUp : PlayerState3D
{
    /// <summary>모션이 끝났는지 여부</summary>
    bool m_isEndLadderUpMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndLadderUpMotion = false;

        m_subController.MoveStopAll();
    }

    private void Update()
    {
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 모션이 끝났을 경우 Idle 상태로 변경
        if(m_isEndLadderUpMotion)
        {
            transform.position = m_subController.CheckLadder.LatelyLadder.UpPosition;
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 사용)</summary>
    public void CompleteLadderUpMotion()
    {
        m_isEndLadderUpMotion = true;
    }
}
