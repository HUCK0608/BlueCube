using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected PlayerManager m_playerManager;
    protected PlayerController m_playerController;

    protected virtual void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();
        m_playerController = GetComponentInParent<PlayerController>();
    }

    public virtual void InitState()
    {

    }

    public virtual void EndState()
    {

    }
}
