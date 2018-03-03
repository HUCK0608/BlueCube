using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat_Monster1 : EnemyStat
{
    [SerializeField]
    private float m_moveSpeed;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return m_moveSpeed; } }

    [SerializeField]
    private float m_rotationSpeed;
    /// <summary>회전속도</summary>
    public float RotationSpeed { get { return m_rotationSpeed; } }

    [SerializeField]
    private float m_chaseRange;
    /// <summary>탐지범위</summary>
    public float ChaseRange { get { return m_chaseRange; } }

    [SerializeField]
    private float m_attackRange;
    /// <summary>공격범위</summary>
    public float AttackRange { get { return m_attackRange; } }

    [SerializeField]
    private float m_returnRange;
    /// <summary>이동범위</summary>
    public float ReturnRange { get { return m_returnRange; } }
}
