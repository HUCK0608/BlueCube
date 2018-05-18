using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    /// <summary>월드 오브젝트</summary>
    protected WorldObject m_worldObject;

    protected bool m_isOn;
    public bool IsOn { get { return m_isOn; } }

    protected virtual void Awake()
    {
        m_worldObject = GetComponentInParent<WorldObject>();
    }
}
