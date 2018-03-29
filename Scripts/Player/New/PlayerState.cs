using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected PlayerManager m_playerManager;
    protected PlayerController m_mainController;

    protected virtual void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();
        m_mainController = GetComponentInParent<PlayerController>();
    }

    public virtual void InitState() { }
    public virtual void EndState() { }
}
