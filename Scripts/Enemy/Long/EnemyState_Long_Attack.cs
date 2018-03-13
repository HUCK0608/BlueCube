using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Long_Attack : EnemyState
{
    private EnemyAttack m_enemyAttack;

    protected override void Awake()
    {
        base.Awake();

        m_enemyAttack = GetComponent<EnemyAttack>();
    }

    public override void InitState()
    {
        m_enemyAttack.Attack();
    }

    private void Update()
    {
        // 공격이끝나면 Move 상태로 변경
        if (!m_enemyAttack.IsAttack)
            m_enemyManager.ChangeState(E_EnemyState.Move);
    }

    public override void EndState()
    {
    }
}
