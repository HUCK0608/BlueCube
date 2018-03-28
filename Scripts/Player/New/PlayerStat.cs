﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat : MonoBehaviour
{
    // 키입력 부분
    [SerializeField]
    private KeyCode m_changeViewKey;
    public KeyCode ChangeViewKey { get { return m_changeViewKey; } }

    [SerializeField] 
    private KeyCode m_pickItemKey;
    public KeyCode PickItemKey { get { return m_pickItemKey; } }

    [SerializeField]
    private KeyCode m_attackKey;
    public KeyCode AttackKey { get { return m_attackKey; } }

    [SerializeField]
    private KeyCode m_jumpKey;
    public KeyCode JumpKey { get { return m_jumpKey; } }

    // 스탯 부분
    [SerializeField]
    private int m_hp;
    public int Hp { get { return m_hp; } }
    public void DecreaseHp(int value) { m_hp -= value; }

    [SerializeField]
    private float m_moveSpeed_Forward;
    public float MoveSpeed_Forward { get { return m_moveSpeed_Forward; } }

    [SerializeField]
    private float m_moveSpeed_SideBack;
    public float MoveSpeed_SideBack { get { return m_moveSpeed_SideBack; } }

    [SerializeField]
    private float m_moveSpeed_Ladder;
    public float MoveSpeed_Ladder { get { return m_moveSpeed_Ladder; } }

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

    [SerializeField]
    private float m_itemCheckDistance;
    public float ItemCheckDistance { get { return m_itemCheckDistance; } }
}
