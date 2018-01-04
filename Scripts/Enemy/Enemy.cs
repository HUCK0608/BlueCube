using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyStat 스크립트가 붙게 만듬
[RequireComponent(typeof(EnemyStat))]
public sealed class Enemy : MonoBehaviour
{
    // 스탯
    private EnemyStat m_stat;
    public EnemyStat Stat { get { return m_stat; } }

    // 죽었는지
    private bool m_isDie;
    public bool IsDie { get { return m_isDie; } }

    private void Awake()
    {
        m_stat = GetComponent<EnemyStat>();
    }

    // 죽음처리
    public void Die()
    {
        m_isDie = true;
        gameObject.SetActive(false);
        Debug.Log(transform.name + " 죽음");
    }
}
