using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerSkill_ChangeView m_skill_CV;
    /// <summary>플레이어 시점변환 스킬</summary>
    public PlayerSkill_ChangeView Skill_CV { get { return m_skill_CV; } }

    private bool m_isMove;
    /// <summary>플레이어가 이동중이면 true를 반환</summary>
    public bool IsMove { get { return m_isMove; } set { m_isMove = value; } }

    private bool m_isJump;
    /// <summary>플레이어가 점프중이면 true를 반환</summary>
    public bool IsJump { get { return m_isJump; } set { m_isJump = value; } }

    private bool m_isGrounded;
    /// <summary>플레이어가 땅위에 있으면 true를 반환</summary>
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }
}
