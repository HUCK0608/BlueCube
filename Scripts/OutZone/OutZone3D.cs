using System.Collections;
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
        // 플레이어가 충돌하면 데미지를 입히고 리스폰 좌표로 이동시킨다
        if (other.tag.Equals(GameLibrary.String_Player))
        {
            m_outZone.HitAndRespawnPlayer();
        }
    }
}
