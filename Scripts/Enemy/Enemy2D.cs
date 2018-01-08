using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Enemy2D : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        // 플레이어가 닿았을 경우 맞았다고 알려줌
        if(other.transform.tag == "PlayerAttack")
        {
            GameManager.Instance.PlayerManager.Stat.Hit(1);
        }
    }
}
