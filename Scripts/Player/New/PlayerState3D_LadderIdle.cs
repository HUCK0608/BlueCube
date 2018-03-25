using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderIdle : PlayerState3D
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        base.InitState();

        m_subController.MoveStopAll();
    }

    private void Update()
    {
        ChangeLadderMove();
    }

    // LadderMove 상태로 변경
    private void ChangeLadderMove()
    {
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 이동 입력이 있을 경우 LadderMove 상태로 변경
        if(!moveDirection.Equals(Vector3.zero))
        {
            m_mainController.ChangeState3D(E_PlayerState.LadderMove);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
