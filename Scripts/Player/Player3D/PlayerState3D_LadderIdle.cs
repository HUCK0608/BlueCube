using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderIdle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        // 전체 속도 멈춤
        m_subController.MoveStopAll();
    }

    private void Update()
    {
        // 상태 변경
        ChangeStates();
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 이동 입력 가져오기
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 이동 입력이 있을 경우 LadderMove 상태로 변경
        if (!moveDirection.Equals(Vector3.zero))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.LadderMove);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
