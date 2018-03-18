using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected PlayerManager m_playerManager;
    protected PlayerController m_playerController;

    protected virtual void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();
        m_playerController = GetComponentInParent<PlayerController>();
    }

    public virtual void InitState()
    {

    }

    protected virtual void Update()
    {
        // 시점변환중이거나 탐지모드일경우 리턴
        if (m_playerManager.isViewChanging || GameManager.Instance.CameraManager.IsObserve)
            return;
    }

    public virtual void EndState()
    {

    }
}
