using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerFoot3D : MonoBehaviour
{
    private PlayerManager m_manager;

    private void Awake()
    {
        m_manager = GetComponentInParent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Bullet")
        {
            // 점프 중이었을 때 아무 물체에나 닿으면
            if (m_manager.IsJumping)
                // 점프상태가 아님을 알림
                m_manager.IsJumping = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Bullet")
        {
            // 땅이라고 알림
            m_manager.IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player" && other.tag != "Bullet")
        {
            m_manager.IsGrounded = false;
        }
    }
}
