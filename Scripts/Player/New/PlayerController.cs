using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 키입력 필드
    [SerializeField]
    private KeyCode m_attackKey;
    /// <summary>공격 키</summary>
    public KeyCode AttackKey { get { return m_attackKey; } }

    [SerializeField]
    private KeyCode m_changeViewKey;
    /// <summary>시점변환 키</summary>
    public KeyCode ChangeViewKey { get { return m_changeViewKey; } }

    [SerializeField]
    private KeyCode m_interactionKey;
    /// <summary>상호작용 키</summary>
    public KeyCode InteractionKey { get { return m_interactionKey; } }

    // 스킬 필드
    private PlayerSkill_ChangeView m_skill_CV;
    /// <summary>플레이어 시점변환 스킬</summary>
    public PlayerSkill_ChangeView Skill_CV { get { return m_skill_CV; } }

    // 캐릭터 상태 관련 필드


    // 캐릭터 상태 bool 필드
    private bool m_isMove;
    /// <summary>플레이어가 이동중이면 true를 반환</summary>
    public bool IsMove { get { return m_isMove; } set { m_isMove = value; } }

    private bool m_isJump;
    /// <summary>플레이어가 점프중이면 true를 반환</summary>
    public bool IsJump { get { return m_isJump; } set { m_isJump = value; } }

    private bool m_isGrounded;
    /// <summary>플레이어가 땅위에 있으면 true를 반환</summary>
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }

    public void Awake()
    {

    }

    // 상태 초기화
    private void InitStates()
    {

    }

    private void Update()
    {

    }
}
