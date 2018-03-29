using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D_Attack : PlayerState2D
{
    private float m_attackAddTime;

    public override void InitState()
    {
        base.InitState();

        Attack();
        m_attackAddTime = 0f;
    }

    private void Attack()
    {
        m_playerManager.Weapon.ShootFireBall2D(m_subController.Forward);
    }

    private void Update()
    {
        ChangeStates();
    }

    // 상태 변경
    private void ChangeStates()
    {
        // 시간 누적
        m_attackAddTime += Time.deltaTime;

        // 모션 딜레이가 지나면 Idle 상태로 변경
        if (m_attackAddTime >= m_playerManager.Stat.AttackMotionDelay)
        {
            m_mainController.ChangeState2D(E_PlayerState2D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
