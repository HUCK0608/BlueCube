using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePanel3D : MonoBehaviour
{
    private static string m_playerTagS = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(m_playerTagS))
        {
            // 플레이어 고정
            GameManager.Instance.PlayerManager.transform.parent = transform.parent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals(m_playerTagS))
        {
            // 플레이어 고정 해제
            GameManager.Instance.PlayerManager.transform.parent = null;
        }
    }
}
