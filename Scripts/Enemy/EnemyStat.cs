using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    // enemyManager
    private EnemyManager m_enemyManager;

    [SerializeField]
    protected int m_hp;

    [SerializeField]
    protected int m_damage;
    public int Damage { get { return m_damage; } }
    
    private void Awake()
    {
        m_enemyManager = GetComponent<EnemyManager>();
    }

    // 피격
    public void Hit(int damage)
    {
        m_hp -= damage;
        
        // 체력이 다 닳았을 경우 죽음 처리
        if (m_hp <= 0)
            m_enemyManager.Die();
    }
}
