﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OutZone3D : MonoBehaviour
{
    private OutZone m_outZone;

    private void Awake()
    {
        m_outZone = GetComponentInParent<OutZone>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 닿았을 경우 체력을 깎고 리스폰
        if (other.tag == "Player")
        {
            GameManager.Instance.PlayerManager.HitAndRespawn(1, m_outZone.RespawnPoint);
        }
    }
}
