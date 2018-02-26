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
        CheckChaseRnage();
    }

    // 추적상태가 되기 위한 거리탐지
    private void CheckChaseRnage()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_player.position);

        if (distanceToPlayer <= m_enemyManager.Stat.ChaseRange)
            m_enemyManager.ChangeState(E_EnemyState_Monster1.Chase);
    }

    public override void EndState()
    {

    }
}
