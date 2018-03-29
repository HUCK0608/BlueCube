using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Long_Idle : EnemyState
{
    private Transform m_player;

    protected override void Awake()
    {
        base.Awake();

        m_player = PlayerManager.Instance.Player3D_Object.transform;
    }

    public override void InitState()
    {
        
    }

    private void Update()
    {
        // 게임 시간이 멈춰있을경우 리턴
        if (GameLibrary.Bool_IsGameStop_Old)
            return;

        // 플레이어가 탐지범위에 들어오면 Move 상태로 변경
        if (m_enemyManager.Stat.DetectionArea.CheckDetected(m_player.position))
            m_enemyManager.ChangeState(E_EnemyState.Move);
    }

    public override void EndState()
    {
    }
}
