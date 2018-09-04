﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckInteraction : MonoBehaviour
{
    private GameObject m_interactionUI;

    private Transform[] m_checkPoints;
    private int m_checkPointCount;
    private int m_layerMask;

    private void Start()
    {
        m_interactionUI = UIManager.Instance.gameObject.transform.Find("InteractionUI").gameObject;
        m_interactionUI.SetActive(false);

        m_checkPoints = PlayerManager.Instance.SubController3D.ItemCheckPoints.ToArray();
        m_checkPointCount = m_checkPoints.Length;
        m_layerMask = GameLibrary.LayerMask_InteractionPickPut | GameLibrary.LayerMask_InteractionPickPut_Bomb | GameLibrary.LayerMask_InteractionPush;
    }

    private void Update()
    {
        if (PlayerManager.Instance.MainController.CurrentState3D.Equals(E_PlayerState3D.Idle) || PlayerManager.Instance.MainController.CurrentState3D.Equals(E_PlayerState3D.Move))
        {
            for (int i = 0; i < m_checkPointCount; i++)
            {
                if (GameLibrary.Raycast3D(m_checkPoints[i].position, m_checkPoints[i].forward, PlayerManager.Instance.Stat.ItemCheckDistance, m_layerMask))
                {
                    m_interactionUI.SetActive(true);
                    return;
                }
            }
        }

        if(m_interactionUI.activeSelf)
            m_interactionUI.SetActive(false);
    }
}
