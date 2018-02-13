using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PE_DamageKnockBack : MonoBehaviour
{
    [SerializeField]
    private float m_knockBackPower_XZ;
    /// <summary>X, Z 넉백파워</summary>
    public float KnockBackPower_XZ { get { return m_knockBackPower_XZ; } }

    [SerializeField]
    private float m_knockBackPower_Y;
    /// <summary>Y 넉백파워</summary>
    public float KnockBackPower_Y { get { return m_knockBackPower_Y; } }

    [SerializeField]
    private float m_damage;
    /// <summary>데미지</summary>
    public float Damage { get { return m_damage; } }

    // 콜라이더 모음
    private Collider m_collider;
    private Collider2D m_collider2D;

    private void Awake()
    {
        m_collider = GetComponentInChildren<Collider>();
        m_collider2D = GetComponentInChildren<Collider2D>();
    }

    private void Start()
    {
        // 데미지 체크 코루틴 시작
        StartCoroutine(DamageCheckTimer());
    }

    /// <summary>언제까지 데미지를 입힐지 체크하는 타이머</summary>
    private IEnumerator DamageCheckTimer()
    {
        float addTime = 0f;
        float checkTime = 0.5f;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= checkTime)
                break;
            yield return null;
        }
        CollidersEnable(false);
    }

    /// <summary>모든 콜라이더 활성화 설정</summary>
    private void CollidersEnable(bool value)
    {
        m_collider.enabled = value;
        m_collider2D.enabled = value;
    }
}
