using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderMove : PlayerState3D
{
    private Vector3 m_moveDirection;

    private float m_angle;

    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 사다리에서 이동
        Move();

        // 상태 변경
        ChangeStates();
    }

    private void Move()
    {
        // 현재 사용중인 사다리
        Ladder currentLadder = m_subController.CheckLadder.LatelyLadder;

        // 이동입력
        m_moveDirection = m_subController.GetMoveDirection();

        // 사다리의 정면방향과 이동방향의 각도를 구함
        m_angle = Vector3.Angle(currentLadder.Forward, m_moveDirection);

        // 각도 체크를 하여 위로 이동
        if (m_angle <= 45f)
            m_subController.LadderMove(Vector3.up);
        // 아래로 이동
        else if (m_angle >= 135f)
            m_subController.LadderMove(Vector3.down);
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 사다리의 제일 아래이거나 맨 위일경우 Move 상태로 변경
        if ((m_subController.CheckLadder.IsLadderDown() && m_angle > 135f) || (!m_subController.CheckLadder.IsOnLadder(m_subController.Forward) && m_angle <= 45f))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Move);
        }
        // 사다리에서 이동입력이 없을경우 LadderIdle 상태로 변경
        else if(m_moveDirection.Equals(Vector3.zero))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.LadderIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
