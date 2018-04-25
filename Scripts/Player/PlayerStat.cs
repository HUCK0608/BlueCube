using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat : MonoBehaviour
{
    // 키입력 부분
    [SerializeField]
    private KeyCode m_changeViewKey;
    public KeyCode ChangeViewKey { get { return m_changeViewKey; } }

    [SerializeField] 
    private KeyCode m_interactionKey;
    public KeyCode InteractionKey { get { return m_interactionKey; } }

    [SerializeField]
    private KeyCode m_acceptKey;
    public KeyCode AcceptKey { get { return m_acceptKey; } }

    [SerializeField]
    private KeyCode m_cancelKey;
    public KeyCode CancelKey { get { return m_cancelKey; } }

    [SerializeField]
    private KeyCode m_jumpKey;
    public KeyCode JumpKey { get { return m_jumpKey; } }

    // 스탯 부분
    [SerializeField]
    private int m_hp;
    public int Hp { get { return m_hp; } }
    public void IncreaseHp(int value) { m_hp += value; }
    public void DecreaseHp(int value) { m_hp -= value; }

    [SerializeField]
    private float m_moveSpeed;
    public float MoveSpeed { get { return m_moveSpeed; } }

    [SerializeField]
    private float m_moveSpeed_Ladder;
    public float MoveSpeed_Ladder { get { return m_moveSpeed_Ladder; } }

    [SerializeField]
    private float m_moveSpeed_Jump;
    public float MoveSpeed_Jump { get { return m_moveSpeed_Jump; } }

    [SerializeField]
    private float m_rotationSpeed;
    public float RotationSpeed { get { return m_rotationSpeed; } }

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
    private float m_itemCheckDistance;
    public float ItemCheckDistance { get { return m_itemCheckDistance; } }

    [SerializeField]
    private float m_objectPickSpeed;
    public float ObjectPickSpeed { get { return m_objectPickSpeed; } }

    [SerializeField]
    private float m_maxHoldTime;
    public float MaxHoldTime { get { return m_maxHoldTime; } }
}
