using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_LadderMove : PlayerState3D
{
    private static string m_ladderMoveSpeed = "LadderMoveSpeed";

    private Vector3 m_moveDirection;

    public override void InitState()
    {
        base.InitState();

        m_subController.Animator.speed = 1f;
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
        m_moveDirection = m_subController.GetMoveDirection();

        if (Input.GetKey(KeyCode.W))
        {
            m_subController.LadderMove(Vector3.up);
            m_subController.Animator.SetFloat(m_ladderMoveSpeed, 1f);
        }
        // 아래로 이동
        else if (Input.GetKey(KeyCode.S))
        {
            m_subController.LadderMove(Vector3.down);
            m_subController.Animator.SetFloat(m_ladderMoveSpeed, -1f);
        }
    }

    /// <summary>상태 변경 모음</summary>
    protected override void ChangeStates()
    {
        // 사다리의 제일 위쪽에 도달할 경우 Ladder Up 상태로 변경
        if(!m_subController.CheckLadder.IsOnLadder(m_subController.Forward) && Input.GetKey(KeyCode.W))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.LadderUp);
        }
        // 사다리의 제일 아래이거나 맨 위일경우 Move 상태로 변경
        else if (m_subController.CheckLadder.IsLadderDown() && Input.GetKey(KeyCode.S))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.LadderDown);
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

        m_subController.Animator.SetFloat(m_ladderMoveSpeed, 0f);
    }
}
