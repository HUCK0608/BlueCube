using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat_Soop : EnemyStat
{
    [SerializeField]
    private EnemyDetectionArea m_detectionArea;
    /// <summary>탐지 공간</summary>
    public EnemyDetectionArea DetectionArea { get { return m_detectionArea; } }

    [SerializeField]
    private float m_rotationSpeed;
    /// <summary>회전 속도</summary>
    public float RotationSpeed { get { return m_rotationSpeed; } }
}
