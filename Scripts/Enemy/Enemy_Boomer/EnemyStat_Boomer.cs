using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat_Boomer : EnemyStat
{
    [SerializeField]
    private float m_rotationSpeed;
    public float RotationSpeed { get { return m_rotationSpeed; } }

    [SerializeField]
    private Transform m_shootPosition;
    public Transform ShootPosition { get { return m_shootPosition; } }

    [SerializeField]
    private float m_shootDelay;
    public float ShootDelay { get { return m_shootDelay; } }

    [SerializeField]
    private float m_startShootMinDelay, m_startShootMaxDelay;
    public float StartShootMinDelay { get { return m_startShootMinDelay; } }
    public float StartShootMaxDelay { get { return m_startShootMaxDelay; } }
}
