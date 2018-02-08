using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_OnOff2D : MonoBehaviour
{
    // 스위치
    private Switch_OnOff m_switch;

    // 플레이어 태그
    private static string m_playerTag = "Player";

    private void Awake()
    {
        m_switch = GetComponentInParent<Switch_OnOff>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 충돌하면 스위치 온
        if (other.transform.tag == m_playerTag)
            m_switch.On();
    }
}
