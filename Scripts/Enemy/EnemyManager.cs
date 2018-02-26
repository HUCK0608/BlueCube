﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 죽었는지
    private bool m_isDie;
    public bool IsDie { get { return m_isDie; } }

    // 죽음처리
    public void Die()
    {
        m_isDie = true;
        gameObject.SetActive(false);
    }
}
