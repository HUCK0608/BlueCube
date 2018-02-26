using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Monster1 : EnemyState
{
    protected EnemyManager_Monster1 m_enemyManager;

    protected virtual void Awake()
    {
        m_enemyManager = GetComponent<EnemyManager_Monster1>();
    }
}
