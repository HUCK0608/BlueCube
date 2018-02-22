using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PE_DamageKnockBack2D : MonoBehaviour
{
    PE_DamageKnockBack m_damageKnockBack;

    private void Awake()
    {
        m_damageKnockBack = GetComponentInParent<PE_DamageKnockBack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 충돌했을 경우
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            // 지금은 데미지만 주겠음
            GameManager.Instance.PlayerManager.Stat.Hit(1);
            
            
        }
        // 적이 충돌했을 경우
        else if(other.tag.Equals(GameLibrary.String_Enemy))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();

            // 적에게 데미지
            enemy.Stat.Hit(1);
        }
    }
}
