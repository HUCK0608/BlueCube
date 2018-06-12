using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected static string m_deadAnimatorStatePath = "Dead";

    [Header("Don't Touch")]
    [SerializeField]
    protected GameObject m_enemy3D;

    [SerializeField]
    protected GameObject m_enemy2D;

    private Collider m_collider3D;
    private Collider2D m_collider2D;

    protected WorldObject m_worldObject;

    protected bool m_isDead;

    protected virtual void Awake()
    {
        m_collider3D = m_enemy3D.GetComponent<Collider>();
        m_collider2D = m_enemy2D.GetComponent<Collider2D>();

        m_worldObject = GetComponent<WorldObject>();
    }

    /// <summary>죽음 로직</summary>
    public virtual void DeadLogic()
    {
        if (!m_isDead)
        {
            m_collider3D.isTrigger = true;
            m_collider2D.isTrigger = true;
            m_collider3D.gameObject.layer = 2;
            m_collider2D.gameObject.layer = 2;
            m_isDead = true;
        }
    }

    public virtual void ChangeEnemy3D() { }
    public virtual void ChangeEnemy2D() { }
}
