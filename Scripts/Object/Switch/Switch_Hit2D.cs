using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_Hit2D : MonoBehaviour
{
    // 부모 스크립트
    private Switch_Hit m_parent;

    private void Awake()
    {
        m_parent = GetComponentInParent<Switch_Hit>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == m_parent.HitType.ToString("G"))
        {
            m_parent.Hit();
        }
    }
}
