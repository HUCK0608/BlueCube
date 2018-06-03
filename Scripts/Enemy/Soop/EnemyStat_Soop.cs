using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat_Soop : EnemyStat
{
    [SerializeField]
    private float m_rotationSpeed;
    /// <summary>회전 속도</summary>
    public float RotationSpeed { get { return m_rotationSpeed; } }

    [SerializeField]
    private float m_shootMinDelay;
    /// <summary>발사 최소 딜레이</summary>
    public float ShootMinDelay { get { return m_shootMinDelay; } }

    [SerializeField]
    private float m_shootMaxDelay;
    /// <summary>발사 최대 딜레이</summary>
    public float ShootMaxDelay { get { return m_shootMaxDelay; } }

    [SerializeField]
    private EnemyDetectionArea m_detectionArea;
    /// <summary>탐지 공간</summary>
    public EnemyDetectionArea DetectionArea { get { return m_detectionArea; } }

    [Header("Don't Touch")]
    [SerializeField]
    private Transform m_shootPosition3D;
    /// <summary>발사 위치</summary>
    public Transform ShootPosition3D { get { return m_shootPosition3D; } }
}
