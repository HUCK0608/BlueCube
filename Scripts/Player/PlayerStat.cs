using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat : MonoBehaviour
{
    private PlayerManager m_playerManager;

    // HP
    [SerializeField]
    private int m_Hp;
    public void HpIncrease(int increaseAmount) { m_Hp += increaseAmount;  Debug.Log("체력증가! 플레이어 체력 : " + m_Hp); }

    // 무적
    private bool m_isInvincibility;

    // 무적시간
    [SerializeField]
    private float m_invincibilityTime;

    // 중력
    [SerializeField]
    private float m_gravity;
    public float Gravity { get { return m_gravity; } }

    // 이동속도
    [SerializeField]
    private float m_forwardMoveSpeed;
    public float ForwardMoveSpeed { get { return m_forwardMoveSpeed; } }

    [SerializeField]
    private float m_sideBackMoveSpeed;
    public float SideBackMoveSpeed { get { return m_sideBackMoveSpeed; } }

    [SerializeField]
    private float m_rotationSpeed;
    public float RotationSpeed { get { return m_rotationSpeed; } }

    // 점프파워
    [SerializeField]
    private float m_jumpPower;
    public float JumpPower { get { return m_jumpPower; } }

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();
    }

    // 피격시 호출
    public void Hit(int damage)
    {
        // 무적일 경우
        if (m_isInvincibility)
            return;

        // 체력감소
        m_Hp -= damage;
        Debug.Log("데미지! 플레이어 체력 : " + m_Hp);

        // 무적시간 코루틴 실행
        StartCoroutine(InvincibilityTime());
    }

    // 무적시간 코루틴
    private IEnumerator InvincibilityTime()
    {
        m_isInvincibility = true;
        yield return new WaitForSeconds(m_invincibilityTime);
        m_isInvincibility = false;
    }
}
