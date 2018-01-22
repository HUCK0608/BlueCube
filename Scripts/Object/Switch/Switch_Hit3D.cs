using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_Hit3D : MonoBehaviour
{
    // 부모 스크립트
    private Switch_Hit m_parent;

    private void Awake()
    {
        m_parent = GetComponentInParent<Switch_Hit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == m_parent.HitType.ToString("G"))
        {
            m_parent.Hit();
        }
    }
}
