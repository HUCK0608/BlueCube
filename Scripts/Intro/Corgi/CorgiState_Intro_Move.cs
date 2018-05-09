using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CorgiState_Intro_Move : CorgiState_Intro
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

    private void ChangeStates()
    {
        Vector3 playerPosition = PlayerController_Intro.Instance.Player.position;
        playerPosition.y = transform.position.y;

        // 최대 다가갈 수 있는 거리안쪽이면 Idle 상태로 변경
        if(Vector2.Distance(transform.position, playerPosition) < m_controller.Stat.MaxDistanceToPlayer)
        {
            m_controller.ChangeStates(E_PlayerState2D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
