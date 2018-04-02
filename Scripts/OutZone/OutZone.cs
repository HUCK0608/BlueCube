using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OutZone : MonoBehaviour
{
    // 리스폰 포인트
    private Vector3 m_respawnPoint;

    private void Awake()
    {
        m_respawnPoint = transform.Find("RespawnPoint").position;
    }

    /// <summary>플레이어에게 데미지를 입히고 리스폰시킴</summary>
    public void HitAndRespawnPlayer()
    {
        PlayerManager.Instance.Hit(1);
        PlayerManager.Instance.Teleport(m_respawnPoint);
    }
}
