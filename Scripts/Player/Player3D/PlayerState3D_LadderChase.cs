using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderChase : PlayerState3D
{
    private Vector3 m_hangPosition;

    public override void InitState()
    {
        base.InitState();

        m_subController.MoveStopXZ();
    }

    private void Update()
    {
        m_hangPosition = m_subController.CheckLadder.LatelyLadder.HangPosition;
        m_hangPosition.y = transform.position.y;

        m_subController.MoveAndRotateTowards(m_hangPosition);

        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 사다리 앞으로 이동했으면 LadderInit 상태로 변경
        if(transform.position.Equals(m_hangPosition))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.LadderInit);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
