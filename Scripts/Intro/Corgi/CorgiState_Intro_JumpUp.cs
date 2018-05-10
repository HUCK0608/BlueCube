using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CorgiState_Intro_JumpUp : CorgiState_Intro
{
    public override void InitState()
    {
        base.InitState();

        m_controller.Jump();
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

    // 상태 변경 모음
    private void ChangeStates()
    {
        if(m_controller.Rigidbody.velocity.y <= 0f)
        {
            m_controller.ChangeStates(E_PlayerState2D.Falling);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
