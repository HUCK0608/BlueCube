using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyStat : MonoBehaviour
{
    // 본체 스크립트
    Enemy m_enemy;

    [SerializeField]
    private int m_hp;
    
    private void Awake()
    {
        m_enemy = GetComponent<Enemy>();
    }

    // 피격
    public void Hit(int damage)
    {
        m_hp -= damage;
        
        // 체력이 다 닳았을 경우 죽음 처리
        if (m_hp <= 0)
            m_enemy.Die();
    }
}
