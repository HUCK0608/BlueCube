using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OutZone2D : MonoBehaviour
{
    private OutZone m_outZone;

    private void Awake()
    {
        m_outZone = GetComponentInParent<OutZone>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 닿았을 경우 체력을 깎고 리스폰
        if(other.tag == "Player")
        {
            // 수정(★)
            //PlayerManager.Instance.HitAndRespawn(1, m_outZone.RespawnPoint);
        }
    }
}
