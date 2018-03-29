using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Attack : PlayerState3D
{
    private float m_attackAddTime;

    public override void InitState()
    {
        base.InitState();

        Attack();
        m_attackAddTime = 0f;
    }

    // 공격
    private void Attack()
    {
        Vector3 mouseDirection = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);
        m_playerManager.PlayerWeapon.ShootFireBall3D(mouseDirection);
    }

    private void Update()
    {
        ChangeIdleState();
    }
    
    // Idle 상태로 변경
    private void ChangeIdleState()
    {
        // 시간 누적
        m_attackAddTime += Time.deltaTime;

        // 모션 딜레이가 지나면 Idle 상태로 변경
        if(m_attackAddTime >= m_playerManager.Stat.AttackMotionDelay)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
