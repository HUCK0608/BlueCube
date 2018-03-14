using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    // enemyManager
    private EnemyManager m_enemyManager;

    private Vector3 m_spawnPosition;
    /// <summary>스폰위치</summary>
    public Vector3 SpawnPosition { get { return m_spawnPosition; } }

    [SerializeField]
    private int m_hp;

    [SerializeField]
    private EnemyDetectionArea m_detectionArea;
    /// <summary>탐지구역</summary>
    public EnemyDetectionArea DetectionArea { get { return m_detectionArea; } }

    [SerializeField]
    private float m_moveSpeed;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return m_moveSpeed; } }

    [SerializeField]
    private float m_rotationSpeed;
    /// <summary>회전속도</summary>
    public float RotationSpeed { get { return m_rotationSpeed; } }

    [SerializeField]
    private float m_maxNearDistance;
    /// <summary>최대로 다가갈 수 있는 거리</summary>
    public float MaxNearDistance { get { return m_maxNearDistance; } }
    
    private void Awake()
    {
        m_enemyManager = GetComponent<EnemyManager>();

        m_spawnPosition = transform.position;
    }

    // 피격
    public virtual void Hit(int damage)
    {
        m_hp -= damage;
        
        // 체력이 다 닳았을 경우 죽음 처리
        if (m_hp <= 0)
            m_enemyManager.Die();
    }
}
