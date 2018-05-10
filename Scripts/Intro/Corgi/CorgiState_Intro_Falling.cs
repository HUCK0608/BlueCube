using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CorgiState_Intro_Falling : CorgiState_Intro
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        MoveToPlayer();

        ChangeStates();
    }

    // 플레이어 방향으로 이동
    private void MoveToPlayer()
    {
        Vector2 directionToPlayer = PlayerController_Intro.Instance.Player.position - transform.position;
        directionToPlayer.y = 0f;

        m_controller.MoveAndRotate(directionToPlayer.normalized);
    }

    // 상태변경 모음
    private void ChangeStates()
    {
        // 땅이면 Idle 상태로 변경
        if(m_controller.IsGrounded)
        {
            m_controller.ChangeStates(E_PlayerState2D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
