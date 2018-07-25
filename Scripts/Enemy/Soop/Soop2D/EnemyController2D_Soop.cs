using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyController2D_Soop : MonoBehaviour
{
    private Animator m_animator;
    /// <summary>애니메이터</summary>
    public Animator Animator { get { return m_animator; } }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    /// <summary>direction 방향을 바라봄</summary>
    public void LookRotate(Vector2 direction)
    {
        Vector2 direction2D = direction;
        direction2D = direction2D.normalized;

        if(!direction2D.x.Equals(transform.localScale.x))
        {
            Vector2 newScale = Vector2.one;
            newScale.x = direction2D.x;

            transform.localScale = newScale;
        }
    }
}
