using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Enemy2D : MonoBehaviour
{
    // 태그검사용 string
    private static string m_playerS = "Player";

    private void OnCollisionStay2D(Collision2D other)
    {
        // 플레이어가 닿았을 경우 맞았다고 알려줌
        if(other.transform.tag == m_playerS)
        {
            GameManager.Instance.PlayerManager.Stat.Hit(1);
        }
    }
}
