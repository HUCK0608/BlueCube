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

    // 점프중 시점전환 할 때 점프속도를 넘기기위한 함수
    public virtual float VelocityY { get; set; }
}
