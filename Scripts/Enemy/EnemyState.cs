using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    protected EnemyManager m_enemyManager;

    protected virtual void Awake()
    {
        m_enemyManager = GetComponent<EnemyManager>();
    }

    public virtual void InitState()
    {

    }

    public virtual void EndState()
    {

    }
}
