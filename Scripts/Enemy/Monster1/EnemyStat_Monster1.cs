using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat_Monster1 : EnemyStat
{
    private EnemyManager_Monster1 m_enemyManager;

    [SerializeField]
    private float m_moveSpeed;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return m_moveSpeed; } }

    [SerializeField]
    private float m_rotationSpeed;
    /// <summary>회전속도</summary>
    public float RotationSpeed { get { return m_rotationSpeed; } }

    [SerializeField]
    private float m_returnMoveSpeed;
    /// <summary>복귀 이동속도</summary>
    public float ReturnMoveSpeed { get { return m_returnMoveSpeed; } }

    [SerializeField]
    private float m_returnRotationSpeed;
    /// <summary>복귀 회전속도</summary>
    public float ReturnRotationSpeed { get { return m_returnRotationSpeed; } }

    [SerializeField]
    private float m_detectionRange;
    /// <summary>탐지범위</summary>
    public float DetectionRange { get { return m_detectionRange; } }

    [SerializeField]
    private float m_chaseRange;
    /// <summary>추적범위</summary>
    public float ChaseRange { get { return m_chaseRange; } }

    [SerializeField]
    private float m_attackRange;
    /// <summary>공격범위</summary>
    public float AttackRange { get { return m_attackRange; } }

    [SerializeField]
    private float m_hitRange;
    /// <summary>피격범위</summary>
    public float HitRange { get { return m_hitRange; } }

    protected override void Awake()
    {
        base.Awake();
        m_enemyManager = GetComponent<EnemyManager_Monster1>();
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
        // 피격당했을시 Chaes State로 변경
        m_enemyManager.ChangeState(E_EnemyState_Monster1.Chase);
    }
}
