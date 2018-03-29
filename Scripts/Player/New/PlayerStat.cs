using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed_Forward;
    public float MoveSpeed_Forward { get { return m_moveSpeed_Forward; } }

    [SerializeField]
    private float m_moveSpeed_SideBack;
    public float MoveSpeed_SideBack { get { return m_moveSpeed_SideBack; } }

    [SerializeField]
    private float m_rotationSpeed;
    public float RotationSpeed { get { return m_rotationSpeed; } }

    [SerializeField]
    private GameObject m_fireBallPrefab;
    public GameObject FireBallPrefab { get { return m_fireBallPrefab; } }

    [SerializeField]
    private float m_fireBallDamage;
    public float FireBallDamage { get { return m_fireBallDamage; } }

    [SerializeField]
    private float m_fireBallSpeed;
    public float FireBallSpeed { get { return m_fireBallSpeed; } }

    [SerializeField]
    private float m_fireBallDurationTime;
    public float FireBallDurationTime { get { return m_fireBallDurationTime; } }
}
