using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Monster1_Idle : EnemyState_Monster1
{
    Transform m_player;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        if (m_player == null)
            m_player = GameManager.Instance.PlayerManager.Player3D_GO.transform;
    }

    private void Update()
    {
        // 게임시간이 멈춘경우 리턴
        if (GameLibrary.Bool_IsGameStop)
            return;

        CheckDetectionRange();
    }

    // 플레이어가 탐지범위안에 들어오면 Chase State로 변경
    private void CheckDetectionRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_player.position);

        if (distanceToPlayer <= m_enemyManager.Stat.DetectionRange)
            m_enemyManager.ChangeState(E_EnemyState_Monster1.Chase);
    }

    public override void EndState()
    {

    }
}
