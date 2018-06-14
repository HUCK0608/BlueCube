using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D_LadderDown : PlayerState3D
{
    // 모션이 끝났는지 여부
    private bool m_isEndLadderDownMotion;

    public override void InitState()
    {
        base.InitState();

        m_isEndLadderDownMotion = false;
    }

    private void Update()
    {
        ChangeStates();
    }

    protected override void ChangeStates()
    {
        if(m_isEndLadderDownMotion)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }

    /// <summary>모션이 끝났다고 설정 (애니메이션 이벤트에서 사용)</summary>
    public void CompleteLadderDownMotion()
    {
        m_isEndLadderDownMotion = true;
    }
}
