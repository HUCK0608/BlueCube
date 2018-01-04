﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Enemy3D : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        // 플레이어가 닿았을 경우 맞았다고 알려줌
        if(other.transform.tag == "Player")
        {
            GameManager.Instance.PlayerManager.Stat.Hit();
        }
    }
}
