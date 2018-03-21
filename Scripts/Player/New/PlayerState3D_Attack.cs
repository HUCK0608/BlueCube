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
        // 시점변환중이거나 탐지시점이면 return
        if (GameLibrary.Bool_IsCO)
            return;

        Vector3 mouseDirection = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);
        m_playerManager.PlayerWeapon.ShootFireBall3D(mouseDirection);
    }

    private void Update()
    {
        CheckDelay();
    }
    
    // 무기 딜레이 체크
    private void CheckDelay()
    {
        m_attackAddTime += Time.deltaTime;

        if(m_attackAddTime >= m_playerManager.Stat.AttackMotionDelay)
        {
            m_mainController.ChangeState3D(E_PlayerState.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
