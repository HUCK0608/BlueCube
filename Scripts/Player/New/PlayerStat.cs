using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat : MonoBehaviour
{
    // 키입력 부분
    [SerializeField]
    private KeyCode m_attackKey;
    public KeyCode AttackKey { get { return m_attackKey; } }

    [SerializeField]
    private KeyCode m_jumpKey;
    public KeyCode JumpKey { get { return m_jumpKey; } }

    // 스탯 부분
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
    private float m_jumpPower;
    public float JumpPower { get { return m_jumpPower; } }

    [SerializeField]
    private float m_gravity;
    public float Gravity { get { return m_gravity; } }

    [SerializeField]
    private float m_attackMotionDelay;
    public float AttackMotionDelay { get { return m_attackMotionDelay; } }
}
