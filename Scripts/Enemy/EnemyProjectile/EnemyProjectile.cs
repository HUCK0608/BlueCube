using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    protected WorldObject m_worldObject;
    protected Rigidbody m_rigidbody;

    protected bool m_isCanUse;
    /// <summary>투사체를 사용할 수 있을경우 true를 반환</summary>
    public bool IsCanUse { get { return m_isCanUse; } }

    protected virtual void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_rigidbody = GetComponentInChildren<Rigidbody>();
    }

    /// <summary>투사체 사용</summary>
    public virtual void UseProjectile(Vector3 origin, Vector3 destination)
    {
        m_isCanUse = false;

        m_worldObject.SetRendererEnable(true);
    }

    /// <summary>발사체 사용을 중지</summary>
    protected void UseStopProjectile()
    {
        m_isCanUse = true;

        m_rigidbody.isKinematic = true;
        m_rigidbody.transform.localPosition = Vector3.zero;
    }
}
