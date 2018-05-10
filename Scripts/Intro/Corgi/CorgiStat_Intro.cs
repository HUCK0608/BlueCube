using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CorgiStat_Intro : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed;
    public float MoveSpeed { get { return m_moveSpeed; } }

    [SerializeField]
    private float m_jumpSpeed;
    public float JumpSpeed { get { return m_jumpSpeed; } }

    [SerializeField]
    private float m_gravity;
    public float Gravity { get { return m_gravity; } }

    [SerializeField]
    private float m_skinWidth;
    public float SkinWidth { get { return m_skinWidth; } }

    [SerializeField]
    private float m_groundCheckDistance;
    public float GroundCheckDistance { get { return m_groundCheckDistance; } }

    [SerializeField]
    private float m_maxDistanceToPlayer;
    public float MaxDistanceToPlayer { get { return m_maxDistanceToPlayer; } }

    [SerializeField]
    private float m_obstacleCheckDistance;
    public float ObstacleCheckDistance { get { return m_obstacleCheckDistance; } }
}
