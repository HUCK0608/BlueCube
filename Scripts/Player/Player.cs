using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 매니저
    protected PlayerManager m_playerManager;

    protected virtual void Awake()
    {
        m_playerManager = transform.parent.GetComponent<PlayerManager>();
    }
}
