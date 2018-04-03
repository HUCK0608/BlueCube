using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderInit : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 머리와 몸이 사다리를 바라보게 만듬
        m_subController.LookRotation(m_subController.CurrentLadder.Forward);

        // 매달릴 위치를 가져와서 적용, y는 현재 위치를 넣어줌
        Vector3 hangPosition = m_subController.CurrentLadder.HangPosition;
        hangPosition.y = transform.position.y;
        transform.position = hangPosition;

        // LadderIdle 상태로 변경
        m_mainController.ChangeState3D(E_PlayerState3D.LadderIdle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
