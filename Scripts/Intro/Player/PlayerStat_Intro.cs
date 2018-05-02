using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat_Intro : MonoBehaviour
{
    [SerializeField]
    private float m_hp;

    [SerializeField]
    private KeyCode m_jumpKey;
    public KeyCode JumpKey { get { return m_jumpKey; } }

    [SerializeField]
    private float m_moveSpeed;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return m_moveSpeed; } }

    [SerializeField]
    private float m_moveSpeed_Jump;
    /// <summary>점프 이동속도</summary>
    public float MoveSpeed_Jump { get { return m_moveSpeed_Jump; } }

    [SerializeField]
    private float m_jumpSpeed;
    /// <summary>점프속도</summary>
    public float JumpSpeed { get { return m_jumpSpeed; } }

    [SerializeField]
    private float m_gravity;
    /// <summary>중력</summary>
    public float Gravity { get { return m_gravity; } }

    [SerializeField]
    private float m_skinWidth;
    /// <summary>피부 크기</summary>
    public float SkinWidth { get { return m_skinWidth; } }

    [SerializeField]
    private float m_groundCheckDistance;
    /// <summary>땅 체크 거리</summary>
    public float GroundCheckDistance { get { return m_groundCheckDistance; } }
}
