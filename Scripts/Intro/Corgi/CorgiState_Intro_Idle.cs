using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CorgiState_Intro_Idle : CorgiState_Intro
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        m_controller.MoveStopX();

        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        Vector3 playerPosition = PlayerController_Intro.Instance.Player.position;
        playerPosition.y = transform.position.y;

        // 땅이 아닐경우 Falling 상태로 변경
        if (!m_controller.IsGrounded)
        {
            m_controller.ChangeStates(E_PlayerState2D.Falling);
        }
        // 플레이어랑 최대 멀어질 수 있는 거리로 멀어질 경우 Move 상태로 변경
        else if(Vector2.Distance(transform.position, playerPosition) >= m_controller.Stat.MaxDistanceToPlayer)
        {
            m_controller.ChangeStates(E_PlayerState2D.Move);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
