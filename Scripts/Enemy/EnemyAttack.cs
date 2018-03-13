using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    protected bool m_isAttack;
    public bool IsAttack { get { return m_isAttack; } }

    public virtual void Attack()
    {
        m_isAttack = true;
    }
}
