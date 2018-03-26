using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderMove : PlayerState3D
{
    private Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        Move();
        ChangeLadderIdleState();
        ChangeMoveState();
    }

    private void Move()
    {
        // 현재 사용중인 사다리
        Ladder currentLadder = m_subController.CurrentLadder;

        // 이동입력
        m_moveDirection = m_subController.GetMoveDirection();

        // 사다리의 정면방향과 이동방향의 각도를 구함
        float angle = Vector3.Angle(currentLadder.Forward, m_moveDirection);

        // 각도 체크를 하여 위로 이동
        if (angle <= 45f)
            m_subController.LadderMove(Vector3.up);
        // 아래로 이동
        else if (angle >= 135f)
                m_subController.LadderMove(Vector3.down);
    }

    // LadderIdle 상태로 변경
    private void ChangeLadderIdleState()
    {
        if (m_moveDirection.Equals(Vector3.zero))
            m_mainController.ChangeState3D(E_PlayerState3D.LadderIdle);
    }

    // Move 상태로 변경
    private void ChangeMoveState()
    {
        // 사다리의 제일 아래이거나 맨 위일경우 Move 상태로 변경
        if (m_subController.CheckLadder.IsLadderDown() || !m_subController.CheckLadder.IsOnLadder(m_subController.Body.forward))
            m_mainController.ChangeState3D(E_PlayerState3D.Move);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
