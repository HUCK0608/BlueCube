using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_HitType { PlayerAttack }

public sealed class Switch_Hit : MonoBehaviour
{
    // 피격 타입
    [SerializeField]
    private E_HitType m_hitType;
    public E_HitType HitType { get { return m_hitType; } }

    // 체력
    [SerializeField]
    private int m_hp;

    // 현재 오브젝트가 부셔졌는지
    private bool m_isBroken;
    public bool IsBroken { get { return m_isBroken; } }

    // 피격 당했을 경우
    public void Hit()
    {
        // 체력감소
        m_hp--;

        // 체력이 없을경우 부셔짐
        if (m_hp <= 0)
        {
            m_isBroken = true;
        }
    }
}
