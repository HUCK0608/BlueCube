using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 매니저
    private PlayerManager m_manager;
    protected PlayerManager Manager { get { return m_manager; } }

    protected virtual void Awake()
    {
        m_manager = transform.parent.GetComponent<PlayerManager>();
    }
}
